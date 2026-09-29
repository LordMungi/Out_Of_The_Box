using UnityEngine;

public struct OnNearbyPartExit: IEvent
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