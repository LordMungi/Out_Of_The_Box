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

        EventBus.Subscribe<OnPartGrabRequest>(OnPartGrabRequest);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnPartGrabRequest>(OnPartGrabRequest);

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

    private void OnPartGrabRequest(in OnPartGrabRequest context)
    {
        _partHandler.GrabAndRelease(context.part);
    }
}
