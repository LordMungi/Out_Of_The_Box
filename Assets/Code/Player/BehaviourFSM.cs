using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourFSM
{
    private Dictionary<Type, PlayerState> _states = new Dictionary<Type, PlayerState>();
    
    public PlayerState currentState;

    public BehaviourFSM(PlayerController p)
    {
        _states.TryAdd(typeof(MoveState), new MoveState(p));
        _states.TryAdd(typeof(JumpState), new JumpState(p));
    }
}
