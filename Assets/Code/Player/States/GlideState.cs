using UnityEngine;
using UnityEngine.InputSystem;

public class GlideState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private bool _hasLeftGround;
    #endregion

    #region Initialization
    public GlideState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        base.Enable();

        _playerInput.Player.Jump.canceled += OnJumpCanceled;
        EventBus.Subscribe<OnPlayerLanded>(OnPlayerLanded);

        _nextState = BehaviourFSM.State.Idle;
        _hasLeftGround = true;
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPlayerLanded>(OnPlayerLanded);
        _playerInput.Player.Jump.canceled -= OnJumpCanceled;

        base.Disable();
    }

    public override void Update()
    {
        _moveHandler.Update();
        
        _moveHandler.GlideFall(_playerConfig.glidingSpeed);

        Vector2 inputDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            _nextState = BehaviourFSM.State.Move;
            _moveHandler.Move(inputDirection, _playerConfig.airborneSpeed, _playerConfig.bodyRotationSpeed);
        }
        else
        {
            _nextState = BehaviourFSM.State.Idle; 
        }
    }

    #endregion

    #region Input Callbacks

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    #endregion

    #region Callbacks

    private void OnPlayerLanded(in OnPlayerLanded playerLandedEvent)
    {
        if (_hasLeftGround)
            EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    #endregion
}