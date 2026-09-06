using UnityEngine;
using UnityEngine.InputSystem;

public class MoveState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;

    private DefaultInputActions _playerInput;


    public MoveState(ref PlayerData p) : base(ref p)
    {
        _handlers.Add(_movementHandler = new MoveHandler(ref p, p.config.moveSpeed));

        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.canceled += OnMoveCanceled;
        _playerInput.Player.Fire.performed += OnJump;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Disable();
        _playerInput.Player.Move.canceled -= OnMoveCanceled; 
        _playerInput.Player.Fire.performed -= OnJump;
        base.Disable();
    }

    public override void Update()
    {
        Debug.Log("Updating MoveState...");
    }

    public override void FixedUpdate()
    {
        _movementHandler.Move(_playerInput.Player.Move.ReadValue<Vector2>());
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Jump;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
