using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private PartHandler _partHandler;

    private PlayerInputActions _playerInput;
    private PlayerConfig _playerConfig;
    #endregion

    #region Initialization
    public IdleState(ref PlayerData data)
    {
        _playerInput = data.input;
        _playerConfig = data.config;

        _handlers.Add(_movementHandler = data.handlers.movement);
        _handlers.Add(_partHandler = data.handlers.parts);
        _handlers.Add(data.handlers.interactions);
    }

    public override void Enable()
    {
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Grab.performed += OnGrabItem;
        _playerInput.Player.Release.performed += OnReleaseItem;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Equip.performed += OnEquipPart;

        EventBus.Subscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);
        EventBus.Subscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);

        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Grab.performed -= OnGrabItem;
        _playerInput.Player.Release.performed -= OnReleaseItem;
        _playerInput.Player.Jump.performed -= OnJump;
        _playerInput.Player.Equip.performed -= OnEquipPart;

        EventBus.Unsubscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        EventBus.Unsubscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);

        base.Disable();
    } 
    #endregion

    public override void Update()
    {
        _movementHandler.Fall(_playerConfig.fallSpeed);
        _movementHandler.Update();
    }

    #region Input Callbacks

    private void OnMove(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Move;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_partHandler.EquippedPart?.type != RobotPart.Type.Jumper)
        {
            EventBus.Raise<OnJumpRequest>();
        }
    }

    private void OnGrabItem(InputAction.CallbackContext context)
    {
        _partHandler.GrabNearest();
    }

    private void OnReleaseItem(InputAction.CallbackContext context)
    {
        _partHandler.Release();
    }

    private void OnEquipPart(InputAction.CallbackContext context)
    {
        _partHandler.EquipGrabbed();
    }
    #endregion

    #region Callbacks
    private void OnJumpRequestAccepted(in OnJumpRequestAccepted context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnPartStateChangeAccepted(in OnPartStateChangeAccepted onPartStateChangeAccepted)
    {
        _nextState = onPartStateChangeAccepted.state;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
    #endregion
}
