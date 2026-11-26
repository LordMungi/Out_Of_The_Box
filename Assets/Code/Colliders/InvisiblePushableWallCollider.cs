using UnityEngine;

public class InvisiblePushableWallCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            Debug.Log("Collision enter");
            EventBus.Raise<OnInvisibleWallCollision>(other.transform.parent.GetComponent<PushableObject>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Grabbable"))
            EventBus.Raise<OnInvisibleWallCollision>(other.transform.parent.GetComponent<PushableObject>());
    }
}