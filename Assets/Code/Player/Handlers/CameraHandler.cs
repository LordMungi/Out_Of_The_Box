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

    private Transform _trackParent;
    
    public CameraHandler(ref CinemachineCamera[] cameras, Transform cameraTrackParent)
    {
        _cameras = new List<CinemachineCamera>(cameras);
        _defaultCamera = _cameras[0];
        SwapToCamera(_defaultCamera);

        _trackParent = cameraTrackParent;
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
                    camera.enabled = true;
                    _currentCamera = camera;
                }
                else
                    camera.enabled = false;
            }
        }
    }

    public void SwapToCamera(string cameraName)
    {
        foreach (CinemachineCamera camera in _cameras)
        {
            if (camera.gameObject.name == cameraName)
            {
                camera.enabled = true;
                _currentCamera = camera;
            }
            else
                camera.enabled = false;
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

    public void ResetCamera()
    {
        _trackParent.position = _trackParent.parent.position;
    }

    public void MoveCamera(Vector3 delta, float speed)
    {
        Vector3 movement = _currentCamera.transform.right * delta.x + _currentCamera.transform.up * delta.y + _currentCamera.transform.forward * delta.z;
        _trackParent.position += movement * Time.deltaTime * speed;
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