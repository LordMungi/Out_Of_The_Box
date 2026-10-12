using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private List<CinemachineCamera> _cameras;
    private CinemachineCamera _defaultCamera;

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
                    camera.enabled = true;
                else
                    camera.enabled = false;
            }
        }
    }

    private void EnterCamera(in OnCameraTriggerEnter context)
    {
        SwapToCamera(context.camera);
    }

    public void ExitCamera(in OnCameraTriggerExit context)
    {
        SwapToCamera(_defaultCamera);
    }
}