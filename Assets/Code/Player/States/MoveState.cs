using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private PartHandler _partHandler;

    private PlayerInputActions _playerInput;
    #endregion

    #region Initialization
    public MoveState(ref PlayerData data, ref PlayerParents parents)
    {
        _playerInput = data.input;

        _handlers.Add(_movementHandler = new MoveHandler(data.controller, data.config.movingMoveData));
        _handlers.Add(_partHandler = new PartHandler(ref parents, _playerInput));
    }
    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Grab.performed += OnGrabItem;
        _playerInput.Player.Release.performed += OnReleaseItem;
        _playerInput.Player.Jump.performed += OnJumpTriggered;
        _playerInput.Player.Equip.performed += OnEquipPart;

        EventBus.Subscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.canceled -= OnMoveCanceled; 
        _playerInput.Player.Grab.performed -= OnGrabItem;
        _playerInput.Player.Release.performed -= OnReleaseItem;
        _playerInput.Player.Jump.performed -= OnJumpTriggered;
        _playerInput.Player.Equip.performed -= OnEquipPart;
        _playerInput.Disable();

        EventBus.Unsubscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);
        base.Disable();
    }
    #endregion

    public override void Update()
    {
        _movementHandler.Move(_playerInput.Player.Move.ReadValue<Vector2>());
        _movementHandler.Fall();
    }

    #region Input Callbacks
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJumpTriggered(InputAction.CallbackContext context)
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
    #endregion
}
