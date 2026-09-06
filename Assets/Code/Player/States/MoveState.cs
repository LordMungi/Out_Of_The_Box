using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    private MovementHandler _movementHandler;

    private DefaultInputActions _playerInput;

    public MoveState(PlayerController p) : base(p)
    {
        _handlers.Add(_movementHandler = new MovementHandler(p));

        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Disable();
        _playerInput.Player.Move.performed -= OnMove; 
        base.Disable();
    }

    public override void Update()
    {
        Debug.Log("Updating MoveState...");
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _movementHandler.Move(context.ReadValue<Vector2>());
    }
}
