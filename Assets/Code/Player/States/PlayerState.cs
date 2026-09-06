using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected PlayerController _player;
    protected List<PlayerHandler> _handlers = new List<PlayerHandler>();
    protected BehaviourFSM.State _nextState;

    public PlayerState(PlayerController p)
    {
        _player = p;
    }

    public abstract void Update();

    public virtual void Enable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Enable();
        }
    }
    public virtual void Disable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Disable();
        }
    }
}
