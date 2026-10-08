using Unity.Cinemachine;
using UnityEngine;

public class StaticCamera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera mainCamera;
    
    private CinemachineCamera _staticCam; 

    private void Awake()
    {
        _staticCam = GetComponent<CinemachineCamera>();
        _staticCam.enabled = false;
    }

    private void Update()
    {
        if (_staticCam.enabled)
        {
            _staticCam.Prioritize();
        }
        else
        {
            mainCamera.Prioritize();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _staticCam.enabled = true;
    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            _staticCam.enabled = false;
    }
}
