using UnityEngine;

public class CameraHandler : PlayerHandler
{
    private Camera[] _cameras;

    public CameraHandler(ref Camera[] cameras)
    {
        _cameras = cameras;
    }
}