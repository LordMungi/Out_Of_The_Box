using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RobotPart : MonoBehaviour
{
    public struct ActionPair
    {
        public Action<InputAction.CallbackContext> inputAction;
        public Action<InputAction.CallbackContext> triggeredAction;
    }

    private Rigidbody _body;

    public List<ActionPair> actions = new List<ActionPair>();

    private void Start()
    {
        _body = GetComponent<Rigidbody>();
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
}
