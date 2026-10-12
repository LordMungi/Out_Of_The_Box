using Unity.Cinemachine;
using UnityEngine;

public struct OnCameraTriggerExit : IEvent
{
    public CinemachineCamera camera;

    public void Assign(params object[] parameters)
    {
        camera = (CinemachineCamera)parameters[0];
    }

    public void Reset()
    {
        camera = null;
    }
}
