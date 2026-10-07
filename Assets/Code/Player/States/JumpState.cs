using UnityEngine;
using UnityEngine.InputSystem;

public class JumpState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private PartHandler _partHandler;

    private PlayerInputActions _playerInput;
    private PlayerConfig _playerConfig;

    public JumpState(ref PlayerData data)
    {
        _playerInput = data.input;
        _playerConfig = data.config;

        _handlers.Add(_movementHandler = data.handlers.movement);
        _handlers.Add(_partHandler = data.handlers.parts);
        _handlers.Add(data.handlers.interactions);
    }

    public override void Enable()
    {
        base.Enable();

        EventBus.Subscribe<OnPlayerLanded>(OnPlayerLanded);

        _nextState = BehaviourFSM.State.Idle;
        _movementHandler.Jump(_playerConfig.jumpForce);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPlayerLanded>(OnPlayerLanded);

        base.Disable();
    }

    public override void Update()
    {
        _movementHandler.Update();
        _movementHandler.Fall(_playerConfig.fallSpeed);

        Vector2 inputDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            _nextState = BehaviourFSM.State.Move;
            _movementHandler.Move(inputDirection, _playerConfig.airborneSpeed);
        }
        else
            _nextState = BehaviourFSM.State.Idle;
    }

    private void OnPlayerLanded(in OnPlayerLanded playerLandedEvent)
    {
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

}
