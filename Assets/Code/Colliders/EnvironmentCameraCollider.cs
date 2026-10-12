using Unity.Cinemachine;
using UnityEngine;

public class EnvironmentCameraCollider : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private CinemachineCamera assignedCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            EventBus.Raise<OnCameraTriggerEnter>(assignedCamera);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            EventBus.Raise<OnCameraTriggerExit>(assignedCamera);
    }
}