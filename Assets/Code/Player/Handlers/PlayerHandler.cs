using UnityEngine;

public abstract class PlayerHandler
{
    protected PlayerData _player;

    public PlayerHandler(ref PlayerData p)
    {
        _player = p;
    }

    public virtual void Enable() { }
    public virtual void Disable() { }
}
