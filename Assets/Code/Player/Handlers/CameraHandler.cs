using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class CameraHandler : PlayerHandler
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private List<CinemachineCamera> _cameras;

    private CinemachineCamera _defaultCamera;

    public CameraHandler(ref CinemachineCamera[] cameras)
    {
        _cameras = new List<CinemachineCamera>(cameras);
        _defaultCamera = _cameras[0];
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
                    camera.gameObject.SetActive(true);
                else
                    camera.gameObject.SetActive(false);
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