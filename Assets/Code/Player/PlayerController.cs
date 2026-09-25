using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private PlayerParents playerParents;

    private PlayerData _playerData;
    private BehaviourFSM _behaviourFSM;

    private void Awake()
    {
        ServiceProvider.Instance.AddService<EventBus>(new EventBus());

        _playerData.player = this;
        _playerData.controller = GetComponent<CharacterController>();
        _playerData.input = new PlayerInputActions();
        _playerData.config = playerConfig;

        _behaviourFSM = new BehaviourFSM(ref _playerData, ref playerParents);
    }

    private void Start()
    {
        EventBus.Raise<OnPlayerInstantiated>(_playerData);
    }

    void Update()
    {
        _behaviourFSM.CurrentState.Update();
    }
}
