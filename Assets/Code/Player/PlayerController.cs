using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private PlayerParents playerParents;

    private BehaviourFSM _behaviourFSM;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());

        PlayerData data;
        data.player = this;
        data.controller = GetComponent<CharacterController>();
        data.input = new PlayerInputActions();
        data.config = playerConfig;
        _behaviourFSM = new BehaviourFSM(ref data, ref playerParents);
    }

    void Update()
    {
        _behaviourFSM.CurrentState.Update();
    }
}
