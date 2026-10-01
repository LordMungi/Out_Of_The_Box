using UnityEngine;
using UnityEngine.InputSystem;

public class SuperJumpState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private PartHandler _partHandler;

    private PlayerInputActions _playerInput;

    private bool _hasLeftGround;
    #endregion

    #region Initialization
    public SuperJumpState(ref PlayerData data)
    {
        _playerInput = data.input;

        _handlers.Add(_movementHandler = data.handlers.movement);
        _handlers.Add(_partHandler = data.handlers.parts);
    }

    public override void Enable()
    {
        base.Enable();

        _playerInput.Player.Jump.canceled += OnJumpReleased;

        EventBus.Subscribe<OnPlayerLanded>(OnPlayerLanded);

        _nextState = BehaviourFSM.State.Idle;
        _hasLeftGround = false;
        _movementHandler.StartSuperJumpCharge();
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
        _movementHandler.Update();
        _movementHandler.Fall();

        Vector2 inputDirection = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDirection != Vector2.zero)
        {
            _nextState = BehaviourFSM.State.Move;
            _movementHandler.Move(inputDirection);
        }
        else
            _nextState = BehaviourFSM.State.Idle;
    }

    #region Input Callbacks
    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        if (!_hasLeftGround)
        {
            _movementHandler.PerformSuperJump();
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
