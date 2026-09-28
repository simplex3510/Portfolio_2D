using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Player.Attack
{
    // AttackInputKey(입력 슬롯) → AttackDataSO(공격 콘텐츠) 매핑 테이블
    // 슬롯이 비어 있으면 해당 입력은 무시된다 (미정 상태를 그대로 표현)
    [CreateAssetMenu(fileName = "New Attack Set", menuName = "Game/Attack/Attack Set")]
    public class AttackSetSO : ScriptableObject
    {
        [Serializable]
        private struct AttackPair
        {
            public AttackInputKey Key;
            public AttackDataSO Data;
        }

        [SerializeField] private AttackPair[] _attackPair;

        private Dictionary<AttackInputKey, AttackDataSO> _attackDict;

        // 컨트롤러가 시작 시 호출하여 조회용 캐시를 준비한다
        public void InitializeAttackDictionary()
        {
            _attackDict = new Dictionary<AttackInputKey, AttackDataSO>(_attackPair.Length);

            foreach (var p in _attackPair)
            {
                if (p.Data == null)
                {
                    continue;
                }

                if (_attackDict.TryAdd(p.Key, p.Data) != true)
                {
                    Debug.LogWarning($"[{name}] there are duplicate AttackInputKey: {p.Key}", this);
                }
            }
        }

        // 슬롯에 대응하는 공격 데이터를 조회한다. 없으면 null (해당 입력 무시)
        public AttackDataSO GetAttackData(AttackInputKey key)
        {
            if (_attackDict == null)
            {
                InitializeAttackDictionary();
            }

            _attackDict.TryGetValue(key, out var data);
            return data;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_attackPair == null)
            {
                return;
            }

            foreach (var p in _attackPair)
            {
                if (p.Data == null)
                {
                    continue;
                }

                bool isHoldSlot = p.Key.PressType == AttackPressType.Hold;
                bool hasExecutionDelay = p.Data.ExecutionThreshold > 0f;

                if (isHoldSlot && !hasExecutionDelay)
                {
                    Debug.LogWarning(
                        $"[{name}] Hold slot({p.Key}) has ExecutionThreshold of 0: {p.Data.name}",
                        this);
                }
            }
        }
#endif
    }
}