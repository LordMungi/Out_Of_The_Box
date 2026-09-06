using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourFSM
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public enum State
    {
        NULL,
        Idle,
        Move,
        Jump
    }

    public PlayerState currentState;

    private Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    private State _currentStateEnum;

    public BehaviourFSM(PlayerController p)
    {
        _states.TryAdd(State.Idle, new IdleState(p));
        _states.TryAdd(State.Move, new MoveState(p));
        _states.TryAdd(State.Jump, new JumpState(p));

        EventBus.Subscribe<OnPlayerStateChangeRequest>(TryChangeState);
        ChangeState(State.Idle);
    }

    public void TryChangeState(in OnPlayerStateChangeRequest newStateEvent)
    {
        bool canChange = false;

        switch (newStateEvent.state)
        {
            case State.NULL:
                break;

            case State.Idle:
                {
                    canChange = _currentStateEnum == State.Move ||
                                _currentStateEnum == State.Jump;
                    break;
                }

            case State.Move:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Jump;
                    break;
                }
            case State.Jump:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Move;
                    break;
                }
        }

        if (canChange)
            ChangeState(newStateEvent.state);
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
