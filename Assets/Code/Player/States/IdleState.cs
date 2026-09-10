using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private DefaultInputActions _playerInput;

    public IdleState(ref PlayerData p)
    {
        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        _playerInput.Player.Fire.performed += OnJump;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Disable();
        _playerInput.Player.Move.performed -= OnMove;
        _playerInput.Player.Fire.performed -= OnJump;
        base.Disable();
    }

    public override void Update()
    {
        Debug.Log("Updating IdleState...");
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

}
