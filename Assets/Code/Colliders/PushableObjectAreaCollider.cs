using UnityEngine;

public class PushableObjectAreaCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            EventBus.Raise<OnPushableObjectEntered>(transform.parent.GetComponent<PushableObject>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            EventBus.Raise<OnPushableObjectExit>(transform.parent.GetComponent<PushableObject>());
    }
}