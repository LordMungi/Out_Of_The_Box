using UnityEngine;
using UnityEngine.InputSystem;

public class FreeCameraState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public FreeCameraState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        base.Enable();

        _playerInput.Player.SwapCamera.performed += OnExitStatePressed;

        _cameraHandler.EnableFreeCamera();
    }

    public override void Disable()
    {
        _cameraHandler.ResetCamera();
        _cameraHandler.DisableFreeCamera();
                
        _playerInput.Player.SwapCamera.performed -= OnExitStatePressed;

        base.Disable();
    }

    public override void Update()
    {
        _cameraHandler.MoveCamera(_playerInput.Player.CameraMove.ReadValue<Vector3>(), _playerConfig.moveSpeed);
    }

    private void OnExitStatePressed(InputAction.CallbackContext callbackContext)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
