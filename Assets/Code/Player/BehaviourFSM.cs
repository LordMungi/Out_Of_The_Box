using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourFSM
{
    public enum State
    {
        Move,
        Jump
    }

    public PlayerState currentState;

    private Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    private State _currentStateEnum;

    public BehaviourFSM(PlayerController p)
    {
        _states.TryAdd(State.Move, new MoveState(p));
        _states.TryAdd(State.Jump, new JumpState(p));

        ChangeState(State.Move);
    }

    public bool TryChangeState(State newState)
    {
        bool canChange = false;

        switch (newState)
        {
            case State.Move:
                {
                    if (_currentStateEnum == State.Jump)
                    {
                        canChange = true;
                    }
                    break;
                }
            case State.Jump:
                {
                    if (_currentStateEnum == State.Move)
                    {
                        canChange = true;
                    }
                    break;
                }
        }

        if (canChange)
            ChangeState(newState);

        return canChange;
    }

    private void ChangeState(State newState)
    {
        currentState?.Disable();

        if (!_states.TryGetValue(newState, out currentState))
            throw new KeyNotFoundException("State not found in Dictionary");

        _currentStateEnum = newState;
        currentState.Enable();
    }
}
