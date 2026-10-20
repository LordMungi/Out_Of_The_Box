using UnityEngine;

public class ClimbableObjectAreaCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            EventBus.Raise<OnClimbableObjectEntered>(transform.parent.GetComponent<ClimbableObject>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            EventBus.Raise<OnClimbableObjectExit>(transform.parent.GetComponent<ClimbableObject>());
    }
}
