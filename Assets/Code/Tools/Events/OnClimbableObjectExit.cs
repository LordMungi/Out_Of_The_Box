public struct OnClimbableObjectExit: IEvent
{
    public ClimbableObject climbableObject;

    public void Assign(params object[] parameters)
    {
        climbableObject = (ClimbableObject)parameters[0];
    }

    public void Reset()
    {
        climbableObject = null;
    }
}
