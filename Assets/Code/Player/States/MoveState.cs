using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

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
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Disable();
        _playerInput.Player.Move.canceled -= OnMoveCanceled; 
        base.Disable();
    }

    public override void Update()
    {
        _movementHandler.Move(_playerInput.Player.Move.ReadValue<Vector2>());
        Debug.Log("Updating MoveState...");
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
