using UnityEngine;
using UnityEngine.InputSystem;

public class SearchingPartState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public SearchingPartState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        base.Enable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _playerInput.Player.Release.performed += OnLeaveState;
    }

    public override void Disable()
    {
        _playerInput.Player.Release.performed -= OnLeaveState;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        base.Disable();
    }

    public override void Update()
    {
    }

    private void OnLeaveState(InputAction.CallbackContext callbackContext)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
