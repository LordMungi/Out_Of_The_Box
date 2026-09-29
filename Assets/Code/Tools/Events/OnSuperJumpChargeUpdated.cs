public struct OnSuperJumpChargeUpdated : IEvent
{
    public float newValue;
    public void Assign(params object[] parameters)
    {
        newValue = (float)parameters[0];
    }

    public void Reset()
    {
        newValue = 0f;
    }
}
