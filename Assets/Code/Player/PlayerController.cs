using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;

    private BehaviourFSM _behaviourFSM;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());

        PlayerData data;
        data.player = this;
        data.body = GetComponent<Rigidbody>();
        data.config = playerConfig;

        _behaviourFSM = new BehaviourFSM(ref data);
    }

    void Update()
    {
        _behaviourFSM.currentState.Update();
    }

    private void FixedUpdate()
    {
        _behaviourFSM.currentState.FixedUpdate();
    }
}
