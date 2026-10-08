using UnityEngine.InputSystem;

public class GliderPart : RobotPart
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    protected override void SetActionPair()
    {
        type = Type.Glider;

        actions.Add(new ActionPair(_inputActions.Player.Jump, ActionPair.Phase.Performed, Glide));
    }

    public void Glide(InputAction.CallbackContext callbackContext)
    {
        EventBus.Raise<OnGlideRequest>();
    }
}
