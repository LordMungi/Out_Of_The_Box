using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _player;
    protected List<PlayerHandler> _handlers;

    public PlayerState(PlayerController p)
    {
        _player = p;
    }

    public abstract void Update();

    public void Enable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Enable();
        }
    }
    public void Disable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Disable();
        }
    }
}
