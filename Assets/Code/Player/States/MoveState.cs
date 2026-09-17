using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private GrabHandler _grabHandler;

    private DefaultInputActions _playerInput;

    public MoveState(ref PlayerData data, ref PlayerParents parents)
    {
        _handlers.Add(_movementHandler = new MoveHandler(data.controller, data.config.movingMoveData));
        _handlers.Add(_grabHandler = new GrabHandler(ref parents));

        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Fire.performed += OnJump;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.canceled -= OnMoveCanceled; 
        _playerInput.Player.Fire.performed -= OnJump;
        _playerInput.Disable();
        base.Disable();
    }

    public override void Update()
    {
        _movementHandler.Move(_playerInput.Player.Move.ReadValue<Vector2>());
        _movementHandler.Fall();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
