using UnityEngine;

namespace Game.Sword
{
    public class SwordFollower : MonoBehaviour
    {
        [SerializeField] private Transform _anchor;
        [SerializeField] private float followSpeed = 5f;
        
        private Vector3 _lastPosition;

        private void Awake()
        {
            if (_anchor == null)
            {
                Debug.LogError("Sword Transform is not assigned in the inspector.");
            }

            _lastPosition = _anchor.position;
        }

        private void LateUpdate()
        {
            Follow();
        }

        private void Follow()
        {
            // _anchor의 null 검사는 Awake에서 이미 수행되었으므로 여기서는 생략

            float t = 1f - Mathf.Exp(-followSpeed * Time.deltaTime);
            Vector3 _currentPosition = Vector3.Lerp(_lastPosition, _anchor.position, t);

            transform.position = _currentPosition;
            _lastPosition = _currentPosition;
        }
    }
}