using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    #endregion

    #region Initialization
    public IdleState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Grab.performed += OnGrabItem;
        _playerInput.Player.Release.performed += OnReleaseItem;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Equip.performed += OnEquipPart;
        _playerInput.Player.SwapCamera.performed += OnSwapCamera;

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
        _playerInput.Player.SwapCamera.performed -= OnSwapCamera;

        EventBus.Unsubscribe<OnPartStateChangeAccepted>(OnPartStateChangeAccepted);
        EventBus.Unsubscribe<OnJumpRequestAccepted>(OnJumpRequestAccepted);

        base.Disable();
    } 
    #endregion

    public override void Update()
    {
        _moveHandler.Fall(_playerConfig.fallSpeed);
        _moveHandler.Update();
    }

    #region Input Callbacks

    private void OnMove(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Move;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (_partHandler.EquippedPartArms?.type != RobotPart.Type.Jumper)
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

    private void OnSwapCamera(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.FreeCamera;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
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
