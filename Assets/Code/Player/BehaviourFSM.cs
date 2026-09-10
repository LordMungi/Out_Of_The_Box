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

    public PlayerState CurrentState { get; private set; }

    private Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    private State _currentStateEnum;

    public BehaviourFSM(ref PlayerData p)
    {
        _states.TryAdd(State.Idle, new IdleState(ref p));
        _states.TryAdd(State.Move, new MoveState(ref p));
        _states.TryAdd(State.Jump, new JumpState(ref p));

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
        CurrentState?.Disable();

        if (!_states.TryGetValue(newState, out PlayerState currentState))
            throw new KeyNotFoundException("State not found in Dictionary");

        CurrentState = currentState;
        _currentStateEnum = newState;
        CurrentState.Enable();
    }
}
