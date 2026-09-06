using UnityEngine;
using UnityEngine.InputSystem;

public class IdleState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private DefaultInputActions _playerInput;

    public IdleState(ref PlayerData p) : base(ref p)
    {
        _playerInput = new DefaultInputActions();
    }

    public override void Enable()
    {
        _playerInput.Enable();
        _playerInput.Player.Move.performed += OnMove;
        base.Enable();
    }

    public override void Disable()
    {
        _playerInput.Disable();
        _playerInput.Player.Move.performed -= OnMove;
        base.Disable();
    }

    public override void Update()
    {
        Debug.Log("Updating IdleState...");
    }
    public override void FixedUpdate()
    {
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Move;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }


}
