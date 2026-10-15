using UnityEngine;
using UnityEngine.InputSystem;

public class SuperJumpState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private bool _hasLeftGround;
    #endregion

    #region Initialization
    public SuperJumpState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        base.Enable();

        _playerInput.Player.Jump.canceled += OnJumpReleased;

        EventBus.Subscribe<OnPlayerLanded>(OnPlayerLanded);

        _nextState = BehaviourFSM.State.Idle;
        _hasLeftGround = false;
        _moveHandler.StartSuperJumpCharge();
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPlayerLanded>(OnPlayerLanded);

        _playerInput.Player.Jump.canceled -= OnJumpReleased;

        base.Disable();
    } 
    #endregion

    public override void Update()
    {
        _moveHandler.Update();
        _moveHandler.UpdateGravity(_playerConfig.fallSpeed);
        if (!_hasLeftGround)
            _moveHandler.UpdateFriction(_playerConfig.decceleration);

        Vector2 inputDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            _nextState = BehaviourFSM.State.Move;
            _moveHandler.Move(inputDirection, _playerConfig.airborneAcceleration, _playerConfig.maxMoveSpeed, _playerConfig.bodyRotationSpeed);
        }
        else
            _nextState = BehaviourFSM.State.Idle;
    }

    #region Input Callbacks
    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        if (!_hasLeftGround)
        {
            _moveHandler.PerformSuperJump(_playerConfig.superJumpMaxTime, _playerConfig.superJumpMaxForce, _playerConfig.superJumpMinForce);
            _hasLeftGround = true;
        }
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
