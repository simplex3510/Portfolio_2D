using System;

using RuneBlade.Network;
using RuneBlade.Simulation;

namespace RuneBlade.Client
{
    /// <summary>
    /// 서버 스냅샷으로 클라이언트 예측을 보정한다(reconciliation).
    ///
    /// 스냅샷마다 상태를 비교하지 않고 항상 같은 절차를 밟는다.
    ///  1. 스냅샷 틱(LastProcessedInputTick)의 상태를 서버 상태로 덮는다.
    ///  2. 그다음 틱부터 가장 최근 틱까지, 이력에 보관한 내 입력을 다시 적용한다.
    ///  3. 결과를 현재 상태로 삼는다.
    /// 서버가 정답이므로 숨은 상태(더블탭 이력, 점프 버퍼 등)의 차이도 이 절차로 함께 해소된다.
    /// 비용은 서버가 아직 확인하지 않은 틱 수(이력 용량 이하)만큼의 Tick() 호출이다.
    ///
    /// "보정됨(Corrected)"은 재적용 전후 현재 위치 차이가 오차 범위를 넘을 때이다(NaN이면 보정으로 본다).
    /// 재적용 중 Tick()이 예외를 던지면(잘못된 설정 등 프로그래밍 오류) 이력이 일부만 갱신된 채 전파된다.
    /// </summary>
    public sealed class MovementReconciler
    {
        // TODO(tuning): 임시값. MoveSpeed 6유닛/초 기준 틱당 이동 0.1유닛의 1%. 실측으로 정한다.
        public const float DefaultPositionTolerance = 0.001f;

        private readonly float _positionTolerance;

        private bool _hasReconciled;
        private int _lastReconciledTick;

        public MovementReconciler(float positionTolerance = DefaultPositionTolerance)
        {
            if (positionTolerance < 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(positionTolerance));
            }

            _positionTolerance = positionTolerance;
        }

        public ReconcileResult Reconcile(
            in SnapshotMessage snapshot,
            PredictionHistory history,
            ref MovementSimulation current,
            in MovementSettings settings)
        {
            int snapshotTick = snapshot.LastProcessedInputTick;

            // 순서가 뒤바뀌어 도착한 오래된 스냅샷은 이미 반영한 결과를 되돌리므로 버린다.
            if (_hasReconciled && snapshotTick <= _lastReconciledTick)
            {
                return ReconcileResult.Ignored;
            }

            // 이력에서 밀려났거나 아직 기록하지 않은 틱이면 재적용할 입력을 알 수 없다.
            if (!history.TryGetState(snapshotTick, out _))
            {
                return ReconcileResult.Ignored;
            }

            // 임시 복사본에서 재적용하고, 끝난 뒤에 현재 상태에 대입한다.
            MovementSimulation replayed = snapshot.State;
            history.ReplaceState(snapshotTick, replayed);

            int newestTick = history.NewestTick;
            for (int tick = snapshotTick + 1; tick <= newestTick; tick++)
            {
                // 기준 틱이 범위 안이면 그 이후 틱도 범위 안이므로 실패하면 이력 구현 오류이다.
                if (!history.TryGetInput(tick, out TickInput input))
                {
                    throw new InvalidOperationException($"재적용할 입력이 이력에 없습니다. 틱={tick}");
                }

                replayed.Tick(input, settings);
                history.ReplaceState(tick, replayed);
            }

            float positionError = Math.Max(
                Math.Abs(replayed.PositionX - current.PositionX),
                Math.Abs(replayed.PositionY - current.PositionY));

            current = replayed;
            _hasReconciled = true;
            _lastReconciledTick = snapshotTick;

            // NaN이면 비교가 모두 false이므로 "범위 안"을 부정형으로 판정해 보정으로 분류한다.
            return !(positionError <= _positionTolerance) ? ReconcileResult.Corrected : ReconcileResult.Matched;
        }
    }
}
