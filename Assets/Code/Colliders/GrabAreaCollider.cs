using UnityEngine;

public class GrabAreaCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnNearbyPartEntered>(other.GetComponent<RobotPart>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnNearbyPartExit>(other.GetComponent<RobotPart>());
    }
}
