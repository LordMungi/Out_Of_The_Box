using System;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourFSM
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public enum State
    {
        Idle,
        Move,
        Jump,
        SuperJump,
        Push,
        NULL
    }

    public PlayerState CurrentState { get; private set; }

    private Dictionary<State, PlayerState> _states = new Dictionary<State, PlayerState>();

    private State _currentStateEnum;

    public BehaviourFSM(ref PlayerData data)
    {
        _states.TryAdd(State.Idle, new IdleState(ref data));
        _states.TryAdd(State.Move, new MoveState(ref data));
        _states.TryAdd(State.Jump, new JumpState(ref data));
        _states.TryAdd(State.SuperJump, new SuperJumpState(ref data));
        _states.TryAdd(State.Push, new PushState(ref data));

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
                                _currentStateEnum == State.Jump ||
                                _currentStateEnum == State.Push ||
                                _currentStateEnum == State.SuperJump;
                    break;
                }

            case State.Move:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Jump ||
                                _currentStateEnum == State.Push ||
                                _currentStateEnum == State.SuperJump;
                    break;
                }
            case State.Jump:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Move;
                    break;
                }

            case State.SuperJump:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Move;
                    break;
                }
            case State.Push:
                {
                    canChange = _currentStateEnum == State.Idle ||
                                _currentStateEnum == State.Move;
                    break;
                }
        }

        if (canChange)
        {
            ChangeState(newStateEvent.state);
            EventBus.Raise<OnPlayerStateChange>(newStateEvent.state);
        }
    }

    private void ChangeState(State newStateEnum)
    {
        CurrentState?.Disable();

        if (!_states.TryGetValue(newStateEnum, out PlayerState newState))
            throw new KeyNotFoundException("State not found in Dictionary");

        CurrentState = newState;
        _currentStateEnum = newStateEnum;
        CurrentState.Enable();
    }
}
