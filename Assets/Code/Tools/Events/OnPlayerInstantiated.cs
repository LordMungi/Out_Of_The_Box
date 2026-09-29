
public struct OnPlayerInstantiated : IEvent
{
    public PlayerData playerData;

    public void Assign(params object[] parameters)
    {
        playerData = (PlayerData)parameters[0];
    }

    public void Reset()
    {
        playerData = default;
    }
}