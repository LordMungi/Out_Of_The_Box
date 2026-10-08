public struct OnPushableObjectExit : IEvent
{
    public PushableObject pushableObject;
    public void Assign(params object[] parameters)
    {
        pushableObject = (PushableObject)parameters[0];
    }

    public void Reset()
    {
        pushableObject = null;
    }
}
