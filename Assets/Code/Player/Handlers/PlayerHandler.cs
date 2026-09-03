using UnityEngine;

public abstract class PlayerHandler : IHandler
{
    private PlayerController player;

    public PlayerHandler(PlayerController p)
    {
        player = p;
    }

    public abstract void Update();
}
