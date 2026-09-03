using UnityEngine;

public abstract class PlayerHandler
{
    protected PlayerController _player;

    public PlayerHandler(PlayerController p)
    {
        _player = p;
    }

    public virtual void Enable() { }
    public virtual void Disable() { }
}
