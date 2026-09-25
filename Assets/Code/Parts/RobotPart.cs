using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class RobotPart : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public struct ActionPair
    {
        public enum Phase
        {
            Started,
            Performed,
            Canceled
        }

        public InputAction inputAction;
        public Phase phase;
        public Action<InputAction.CallbackContext> triggeredAction;

        public ActionPair(InputAction inputAction,  Phase phase, Action<InputAction.CallbackContext> triggeredAction)
        {
            this.inputAction = inputAction;
            this.phase = phase;
            this.triggeredAction = triggeredAction;
        }
    }

    private Rigidbody _body;
    protected PlayerInputActions _inputActions;

    public List<ActionPair> actions = new List<ActionPair>();

    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        EventBus.Subscribe<OnPlayerInstantiated>(GetPlayerData);
    }

    public void GetPlayerData(in OnPlayerInstantiated onPlayerInstantiated)
    {
        _inputActions = onPlayerInstantiated.playerData.input;
        SetActionPair();
    }

    public void Grab()
    {
        _body.isKinematic = true;
        _body.useGravity = false;
    }

    public void Release()
    {
        _body.isKinematic = false;
        _body.useGravity = true;
        _body.linearVelocity = Vector3.zero;
    }

    public void Equip()
    {
        _body.isKinematic = true;
        _body.useGravity = false;
    }

    protected abstract void SetActionPair();
}
