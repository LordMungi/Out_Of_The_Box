using UnityEngine.InputSystem;

public class PauseState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public PauseState(ref PlayerData data) : base(ref data){}

    public override void Enable()
    {
        EventBus.Subscribe<OnPlayerUnpauseAccepted>(OnUnpauseAccepted);
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        EventBus.Unsubscribe<OnPlayerUnpauseAccepted>(OnUnpauseAccepted);
    }

    public override void Update()
    {
    }

    protected override void OnPause(InputAction.CallbackContext context)
    {
        EventBus.Raise<OnPlayerUnpauseRequest>();
    }

    private void OnUnpauseAccepted(in OnPlayerUnpauseAccepted context)
    {
        _nextState = BehaviourFSM.LastState;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
