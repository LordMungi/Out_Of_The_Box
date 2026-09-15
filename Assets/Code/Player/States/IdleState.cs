using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;

    private DefaultInputActions _playerInput;

    public IdleState(ref PlayerData p)
    {
        _playerInput = new DefaultInputActions();

        _handlers.Add(_movementHandler = new MoveHandler(p.controller, p.config.movingMoveData));
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Fire.performed += OnJump;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Fire.performed -= OnJump;
        _playerInput.Disable();
        base.Disable();
    }

    public override void Update()
    {
        _movementHandler.Fall();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Move;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

}
