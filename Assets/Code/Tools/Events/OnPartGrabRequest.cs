public struct OnPartGrabRequest : IEvent
{
    public RobotPart part;
    public void Assign(params object[] parameters)
    {
        part = (RobotPart)parameters[0];
    }

    public void Reset()
    {
        part = null;
    }
}
