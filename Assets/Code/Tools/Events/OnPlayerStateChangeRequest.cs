public struct OnPlayerStateChangeRequest : IEvent
{
    public BehaviourFSM.State state;

    public void Assign(params object[] parameters)
    {
        state = (BehaviourFSM.State)parameters[0];
    }

    public void Reset()
    {
        state = BehaviourFSM.State.NULL;
    }
}
