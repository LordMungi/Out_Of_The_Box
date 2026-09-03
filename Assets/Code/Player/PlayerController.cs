using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BehaviourFSM _behaviourFSM;

    private void Awake()
    {
        _behaviourFSM = new BehaviourFSM(this);
    }

    void Update()
    {
        _behaviourFSM.currentState.Update();
    }
}
