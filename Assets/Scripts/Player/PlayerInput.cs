using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInput : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private InputActionReference _moveActRef;

    public Vector2 Input { get; private set; } = Vector2.zero;

    private void Awake()
    {
        if (_moveActRef == null)
        {
            Debug.LogError("Move Action Reference is not assigned in the inspector.", this);
            enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        _moveActRef.action.Enable();
    }

    private void OnDisable()
    {
        _moveActRef.action.Disable();
    }

    private void Update()
    {
        UpdateMoveInput();
    }

    #region Update Relative Methods
    private void UpdateMoveInput()
    {
        Input = _moveActRef.action.ReadValue<Vector2>();
    }

    private void UpdateLeftClickInput()
    {
        // Implement mouse input handling if needed
    }
    #endregion
}
