using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Serializable]
    public struct BoneConfig
    {
        public Transform bone;
        public Vector3 axisOffset;
        public float rotationSpeed;
    }
    
    [Header("Bone Config")]
    public BoneConfig[] bodyBones;
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private PlayerConfig playerConfig;
    [SerializeField] private PlayerParents playerParents;

    private PlayerData _playerData;
    private BehaviourFSM _behaviourFSM;
    private MouseTargetPosition _mouseTargetPosition;

    private void Awake()
    {
        _playerData.player = this;
        _playerData.controller = GetComponent<CharacterController>();
        _playerData.input = new PlayerInputActions();
        _playerData.input.Enable();
        _playerData.config = playerConfig;

        _behaviourFSM = new BehaviourFSM(ref _playerData, ref playerParents);
        _mouseTargetPosition = new MouseTargetPosition(bodyBones, ref playerParents);
    }

    private void Start()
    {
        EventBus.Raise<OnPlayerInstantiated>(_playerData);
    }

    void Update()
    {
        _behaviourFSM.CurrentState.Update();
        _mouseTargetPosition.Update();
    }
}
