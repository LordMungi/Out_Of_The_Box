using UnityEngine;

public class ClimbableObjectAreaCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            EventBus.Raise<OnClimbableObjectEntered>(transform.parent.GetComponent<ClimbableObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exit");
            EventBus.Raise<OnClimbableObjectExit>(transform.parent.GetComponent<ClimbableObject>());
        }
    }
}
