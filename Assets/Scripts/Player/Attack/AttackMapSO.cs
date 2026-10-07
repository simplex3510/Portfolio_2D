using System;
using System.Collections.Generic;

using UnityEngine;

using Game.Input.Attack;

namespace Game.Player.Attack
{

    /// <summary>
    /// 입력(AttackInput)에 공격(AttackDataSO)을 대응시키는 할당표.
    /// 인스펙터에서 슬롯마다 입력 값과 공격 SO를 지정하고, 런타임에는 입력 값으로 공격을 조회한다.
    /// 어느 시점에 공격을 확정할지는 슬롯에 어떤 입력(Gesture, Action)을 지정하느냐로 정해진다.
    /// 무기가 Sword 하나뿐이므로 프로젝트 전체에서 하나만 사용한다.
    /// </summary>
    [CreateAssetMenu(fileName = "AttackMap", menuName = "ScriptableObject/Attack/Attack Map")]
    public class AttackMapSO : ScriptableObject
    {
        /// <summary>입력 값 하나와 그에 할당된 공격의 쌍</summary>
        [Serializable]
        public struct Slot
        {
            [SerializeField] private AttackInput _input;
            [SerializeField] private AttackDataSO _attack;

            public readonly AttackInput Input { get { return _input; } }
            public readonly AttackDataSO Attack { get { return _attack; } }
        }

        [SerializeField] private List<Slot> _slotList = new();

        // 조회용 캐시. AttackInput이 IEquatable을 구현하므로 키 비교와 조회에서 박싱 할당이 없다.
        private Dictionary<AttackInput, AttackDataSO> _attackMap;

        /// <summary>입력 값에 할당된 공격을 찾는다. 할당이 없으면 false.</summary>
        public bool TryGetAttackData(AttackInput input, out AttackDataSO attack)
        {
            if (_attackMap == null)
            {
                attack = null;
                return false;
            }

            return _attackMap.TryGetValue(input, out attack);
        }

        private void BuildAttackMap()
        {
            _attackMap = new Dictionary<AttackInput, AttackDataSO>();

            for (int i = 0; i < _slotList.Count; i++)
            {
                Slot slot = _slotList[i];

                // 공격이 비어 있는 슬롯은 무시한다
                if (slot.Attack == null)
                    continue;

                // 같은 입력이 중복 등록되면 먼저 등록된 슬롯을 사용한다
                if (_attackMap.ContainsKey(slot.Input))
                {
                    Debug.LogWarning($"AttackMapSO: 중복된 입력 '{slot.Input}' 슬롯입니다. 먼저 등록된 슬롯을 사용합니다.", this);
                    continue;
                }

                _attackMap.Add(slot.Input, slot.Attack);
            }
        }

#if UNITY_EDITOR
        // 인스펙터에서 슬롯을 수정하면 캐시를 다시 만든다 (중복 경고를 편집 즉시 확인할 수 있다)
        private void OnValidate()
        {
            BuildAttackMap();
        }
#endif
    }
}
