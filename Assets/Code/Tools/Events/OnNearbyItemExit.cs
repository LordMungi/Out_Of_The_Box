using UnityEngine;

public struct OnNearbyItemExit: IEvent
{
    public RobotPart item;
    public void Assign(params object[] parameters)
    {
        item = (RobotPart)parameters[0];
    }

    public void Reset()
    {
        item = null;
    }
}