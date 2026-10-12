public struct OnGrabRequest : IEvent
{
    public RobotPart grabbedPart;
    public void Assign(params object[] parameters)
    {
        grabbedPart = (RobotPart)parameters[0];
    }

    public void Reset()
    {
        grabbedPart = null;
    }
}