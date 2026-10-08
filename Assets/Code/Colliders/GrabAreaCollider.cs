using UnityEngine;

public class GrabAreaCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnNearbyPartEntered>(other.GetComponent<RobotPart>());
        if (other.CompareTag("Pushable"))
            EventBus.Raise<OnPushableObjectEntered>(other.GetComponent<PushableObject>());
        if (other.CompareTag("Climbable"))
            EventBus.Raise<OnClimbableObjectEntered>(other.GetComponent<ClimbableObject>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnNearbyPartExit>(other.GetComponent<RobotPart>());
        if (other.CompareTag("Pushable"))
            EventBus.Raise<OnPushableObjectExit>(other.GetComponent<PushableObject>());
        if (other.CompareTag("Climbable"))
            EventBus.Raise<OnClimbableObjectExit>(other.GetComponent<ClimbableObject>());
    }
}
