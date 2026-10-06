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
    [SerializeField] private Animator playerAnimator;

    private PlayerData _playerData;
    private BehaviourFSM _behaviourFSM;
    private PlayerAnimator _playerAnimationController;
    private MouseTargetPosition _mouseTargetPosition;

    private void Awake()
    {
        _playerData.player = this;
        _playerData.controller = GetComponent<CharacterController>();
        _playerData.input = new PlayerInputActions();
        _playerData.input.Enable();
        _playerData.config = playerConfig;
        _playerData.parents = playerParents;

        _playerData.handlers.movement = new MoveHandler(_playerData.controller);
        _playerData.handlers.parts = new PartHandler(ref _playerData.parents, _playerData.input);

        _playerAnimationController = new PlayerAnimator(playerAnimator);
        _behaviourFSM = new BehaviourFSM(ref _playerData);

        _mouseTargetPosition = new MouseTargetPosition(bodyBones, ref _playerData.parents);
    }

    private void Start()
    {
        EventBus.Raise<OnPlayerInstantiated>(_playerData);
    }

    void Update()
    {
        if (FreeCam.IsActive)
            return;
        
        _behaviourFSM.CurrentState.Update();
        _mouseTargetPosition.Update();
    }
}
