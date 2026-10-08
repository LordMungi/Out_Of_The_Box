using UnityEngine.InputSystem;

public class ClimbState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private MoveHandler _movementHandler;
    private InteractionsHandler _interactionsHandler;

    private PlayerInputActions _playerInput;
    private PlayerConfig _playerConfig;

    public ClimbState(ref PlayerData data)
    {
        _playerInput = data.input;
        _playerConfig = data.config;

        _handlers.Add(_movementHandler = data.handlers.movement);
        _handlers.Add(data.handlers.parts);
        _handlers.Add(_interactionsHandler = data.handlers.interactions);
    }

    public override void Enable()
    {
        base.Enable();
        _playerInput.Player.Release.performed += StopClimb;
    }

    public override void Disable()
    {
        base.Disable();
    }

    public override void Update()
    {
    }

    private void StopClimb(InputAction.CallbackContext callbackContext)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
