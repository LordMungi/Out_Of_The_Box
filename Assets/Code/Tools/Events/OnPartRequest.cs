public struct OnPartRequest : IEvent
{
    public RobotPart.Type type;
    public void Assign(params object[] parameters)
    {
        type = (RobotPart.Type)parameters[0];
    }

    public void Reset()
    {
        type = RobotPart.Type.ArmBase;
    }
}
