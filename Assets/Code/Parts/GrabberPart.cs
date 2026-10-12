using UnityEngine;
using UnityEngine.InputSystem;

public class GrabberPart : RobotPart
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    protected override void SetActionPair()
    {
        type = Type.Grabber;

        actions.Add(new ActionPair(_inputActions.Player.Attack, ActionPair.Phase.Performed, Grab));
    }

    public void Grab(InputAction.CallbackContext callbackContext)
    {
        EventBus.Raise<OnPartStateChangeAccepted>(BehaviourFSM.State.Grab);
    }

    public void PartDetected(RobotPart part)
    {
        EventBus.Raise<OnGrabRequest>(part);
    }

    public void WallDetected()
    {
        EventBus.Raise<OnGrabberHitWall>();
    }
}