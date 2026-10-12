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
    
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    [SerializeField] private PlayerData _playerData;

    private BehaviourFSM _behaviourFSM;

    private PlayerAnimator _playerAnimationController;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerData.input = new PlayerInputActions();
        _playerData.input.Enable();

        _playerData.handlers.movement = new MoveHandler(_playerData.controller, _playerData.rootBone);
        _playerData.handlers.parts = new PartHandler(ref _playerData.parents, _playerData.input);
        _playerData.handlers.interactions = new InteractionsHandler(ref _playerData.parents);
        _playerData.handlers.camera = new CameraHandler(ref _playerData.cameras, _playerData.parents.cameraTrackParent);

        _playerAnimationController = new PlayerAnimator(_playerData.animator); // Convertir en handler

        _behaviourFSM = new BehaviourFSM(ref _playerData);
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
    }
}
