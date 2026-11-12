using UnityEngine.InputSystem;

public class PauseState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public PauseState(ref PlayerData data) : base(ref data){}

    public override void Update()
    {
    }

    protected override void OnPause(InputAction.CallbackContext context)
    {
        EventBus.Raise<OnPlayerUnpause>();
        _nextState = BehaviourFSM.LastState;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
