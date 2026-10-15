using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    #endregion

    #region Initialization
    public MoveState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Grab.performed += OnInteract;
        _playerInput.Player.Release.performed += OnReleaseItem;
        _playerInput.Player.Jump.performed += OnJumpTriggered;
        _playerInput.Player.Equip.performed += OnEquipPart;

        EventBus.Subscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);
        EventBus.Subscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.canceled -= OnMoveCanceled; 
        _playerInput.Player.Grab.performed -= OnInteract;
        _playerInput.Player.Release.performed -= OnReleaseItem;
        _playerInput.Player.Jump.performed -= OnJumpTriggered;
        _playerInput.Player.Equip.performed -= OnEquipPart;

        EventBus.Unsubscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        EventBus.Unsubscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);
        base.Disable();
    }
    #endregion

    public override void Update()
    {
        _moveHandler.Move(_playerInput.Player.Move.ReadValue<Vector2>(), _playerConfig.moveSpeed, _playerConfig.bodyRotationSpeed);
        _moveHandler.Update();
        _moveHandler.UpdateGravity(_playerConfig.fallSpeed);
        _moveHandler.UpdateFriction(_playerConfig.decelerationSpeed);
    }

    #region Input Callbacks
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJumpTriggered(InputAction.CallbackContext context)
    {
        if (_partHandler.EquippedPartArms?.type != RobotPart.Type.Jumper)
        {
            EventBus.Raise<OnJumpRequest>();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!_partHandler.GrabNearest())
        {
            switch (_interactionsHandler.Interact())
            {
                case InteractionsHandler.Type.Toybox:
                    _nextState = BehaviourFSM.State.SearchingPart;
                    EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
                    break;
            }
        }
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
