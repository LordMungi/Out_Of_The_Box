using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private List<CinemachineCamera> _cameras;
    private CinemachineCamera _defaultCamera;
    private CinemachineCamera _currentCamera;
    private CinemachineCamera _previousCamera;
        
    public CameraHandler(ref CinemachineCamera[] cameras)
    {
        _cameras = new List<CinemachineCamera>(cameras);
        _defaultCamera = _cameras[0];
        SwapToCamera(_defaultCamera);
    }

    public override void Enable()
    {
        base.Enable();

        EventBus.Subscribe<OnCameraTriggerEnter>(EnterCamera);
        EventBus.Subscribe<OnCameraTriggerExit>(ExitCamera);
    }

    public override void Disable()
    {
        EventBus.Unsubscribe<OnCameraTriggerEnter>(EnterCamera);
        EventBus.Unsubscribe<OnCameraTriggerExit>(ExitCamera);
        
        base.Disable();
    }

    public void SwapToCamera(CinemachineCamera newCamera)
    {
        if (_cameras.Contains(newCamera))
        {
            foreach (CinemachineCamera camera in _cameras)
            {
                if (camera == newCamera)
                {
                    camera.gameObject.SetActive(true);
                    _currentCamera = camera;
                }
                else
                    camera.gameObject.SetActive(false);
            }
        }
    }

    public void SwapToCamera(string cameraName)
    {
        foreach (CinemachineCamera camera in _cameras)
        {
            if (camera.gameObject.name == cameraName)
            {
                camera.gameObject.SetActive(true);
                _currentCamera = camera;
            }
            else
                camera.gameObject.SetActive(false);
        }
    }

    public void EnableFreeCamera()
    {
        _previousCamera = _currentCamera;
        SwapToCamera("Free");
    }

    public void DisableFreeCamera()
    {
        SwapToCamera(_previousCamera);
        _previousCamera = null;
    }

    public void MoveCamera(Vector3 delta, float speed)
    {
        Vector3 movement = _currentCamera.transform.right * delta.x + _currentCamera.transform.up * delta.y + _currentCamera.transform.forward * delta.z;
        _currentCamera.Target.TrackingTarget.position += movement * Time.deltaTime * speed;
    }

    private void EnterCamera(in OnCameraTriggerEnter context)
    {
        SwapToCamera(context.camera);
    }

    private void ExitCamera(in OnCameraTriggerExit context)
    {
        SwapToCamera(_defaultCamera);
    }
}