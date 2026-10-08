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
        EventBus.Subscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        EventBus.Subscribe<OnGlideRequestAccepted>(OnGlideRequestAccepted);
        
        _nextState = BehaviourFSM.State.Idle;
        
        if (_movementHandler.IsGrounded)
        {
            _movementHandler.Jump(_playerConfig.jumpForce);
        }
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPlayerLanded>(OnPlayerLanded);
        EventBus.Unsubscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        EventBus.Unsubscribe<OnGlideRequestAccepted>(OnGlideRequestAccepted);

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
        
        if (_partHandler.EquippedPart?.type == RobotPart.Type.Glider)
        {
            if (_playerInput.Player.Jump.IsPressed())
            {
                EventBus.Raise<OnGlideRequest>(); 
            }
        }
    }

    private void OnPlayerLanded(in OnPlayerLanded playerLandedEvent)
    {
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnGlideRequestAccepted(in OnGlideRequestAccepted playerGlideRequestAcceptedEvent)
    {
        _nextState = BehaviourFSM.State.Glide;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    #region Callbacks
    
    private void OnPartStateChangeAccepted(in OnPartStateChangeAccepted onPartStateChangeAccepted)
    {
        _nextState = onPartStateChangeAccepted.state;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
    
    #endregion
}
