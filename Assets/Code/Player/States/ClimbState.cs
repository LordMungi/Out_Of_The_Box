using UnityEngine;
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

        _interactionsHandler.ClimbNearestObject();
        _movementHandler.SetPosition(_interactionsHandler.GetOffsetedPosition(_movementHandler.Position));
    }

    public override void Disable()
    {
        if (_interactionsHandler.ShouldGetOffOnTop())
            _movementHandler.SetPosition(_interactionsHandler.GetOffsetedTopPosition(_movementHandler.Position));

        _interactionsHandler.StopClimbing();

        base.Disable();
    }

    public override void Update()
    {
        Vector2 inputDelta = _playerInput.Player.Move.ReadValue<Vector2>();

        if (!_interactionsHandler.GrabbedClimbableObject.IsPlayerOnTop)
            _movementHandler.MoveLinear(new Vector3(0, inputDelta.y, 0) * Time.deltaTime * _playerConfig.climbSpeed);

        _movementHandler.Update();
    }

    private void StopClimb(InputAction.CallbackContext callbackContext)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
