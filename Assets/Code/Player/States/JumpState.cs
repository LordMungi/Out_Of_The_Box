using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private JumpHandler _jumpHandler;

    private DefaultInputActions _playerInput;

    public JumpState(ref PlayerData p)
    {
        _handlers.Add(_movementHandler = new MoveHandler(p.controller, p.config.jumpingMoveData));
        _handlers.Add(_jumpHandler = new JumpHandler(p.controller, p.config.jumpingJumpData));

        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        base.Enable();

        EventBus.Subscribe<OnPlayerLanded>(OnPlayerLanded);
        
        _playerInput.Enable();

        _nextState = BehaviourFSM.State.Idle;
        _jumpHandler.Jump();
    }

    public override void Disable()
    {
        _playerInput.Disable();

        EventBus.Unsubscribe<OnPlayerLanded>(OnPlayerLanded);
        base.Disable();
    }

    public override void Update()
    {
        _jumpHandler.Fall();

        Vector2 inputDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            _nextState = BehaviourFSM.State.Move;
            _movementHandler.Move(inputDirection);
        }
        else
            _nextState = BehaviourFSM.State.Idle;
    }

    private void OnPlayerLanded(in OnPlayerLanded playerLandedEvent)
    {
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

}
