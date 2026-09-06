using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private BehaviourFSM _behaviourFSM;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());

        _behaviourFSM = new BehaviourFSM(this);
    }

    void Update()
    {
        _behaviourFSM.currentState.Update();
    }
}
