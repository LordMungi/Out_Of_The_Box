using UnityEngine.InputSystem;

public class ClimberPart : RobotPart
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    protected override void SetActionPair()
    {
        type = Type.Climber;

        actions.Add(new ActionPair(_inputActions.Player.Grab, ActionPair.Phase.Performed, RequestClimb));
    }

    private void RequestClimb(InputAction.CallbackContext callbackContext)
    {
        EventBus.Raise<OnClimbRequest>();
    }
}