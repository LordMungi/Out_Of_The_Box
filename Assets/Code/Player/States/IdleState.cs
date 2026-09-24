using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private PartHandler _partHandler;

    private PlayerInputActions _playerInput;

    public IdleState(ref PlayerData data, ref PlayerParents parents)
    {
        _playerInput = new PlayerInputActions();

        _handlers.Add(_movementHandler = new MoveHandler(data.controller, data.config.movingMoveData));
        _handlers.Add(_partHandler = new PartHandler(ref parents, _playerInput));
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Grab.performed += OnGrabItem;
        _playerInput.Player.Release.performed += OnReleaseItem;
        _playerInput.Player.Jump.performed += OnJump;
        _playerInput.Player.Equip.performed += OnEquipPart;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Grab.performed -= OnGrabItem;
        _playerInput.Player.Release.performed -= OnReleaseItem;
        _playerInput.Player.Jump.performed -= OnJump;
        _playerInput.Player.Equip.performed -= OnEquipPart;
        _playerInput.Disable();
        base.Disable();
    }

    public override void Update()
    {
        _movementHandler.Fall();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Move;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnGrabItem(InputAction.CallbackContext context)
    {
        _partHandler.GrabNearest();
    }

    private void OnReleaseItem(InputAction.CallbackContext context)
    {
        _partHandler.Release();
    }

    private void OnEquipPart(InputAction.CallbackContext context)
    {
        _partHandler.EquipGrabbed();
    }
}
