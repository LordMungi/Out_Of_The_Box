using UnityEngine;

public class InvisiblePushableWallCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnInvisibleWallCollision>(transform.parent.GetComponent<ClimbableObject>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnInvisibleWallCollision>(transform.parent.GetComponent<ClimbableObject>());
    }
}