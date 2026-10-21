using UnityEngine;
using UnityEngine.InputSystem;

public class ClimbState : PlayerState
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public ClimbState(ref PlayerData data) : base(ref data) { }

    public override void Enable()
    {
        base.Enable();
        _playerInput.Player.Release.performed += StopClimb;

        _interactionsHandler.ClimbNearestObject();
        _moveHandler.SetPosition(_interactionsHandler.GetOffsetedPosition(_moveHandler.Position));
        _moveHandler.SetRotation(_interactionsHandler.GetClimbingRotation());

    }

    public override void Disable()
    {
        if (_interactionsHandler.ShouldGetOffOnTop())
            _moveHandler.SetPosition(_interactionsHandler.GetOffsetedTopPosition(_moveHandler.Position));

        _moveHandler.SetRotation(_interactionsHandler.GetTopRotation());

        _interactionsHandler.StopClimbing();

        base.Disable();
    }

    public override void Update()
    {
        Vector2 inputDelta = _playerInput.Player.Move.ReadValue<Vector2>();

        if ((!_interactionsHandler.GrabbedClimbableObject.IsPlayerOnTop || inputDelta.y < 0) && inputDelta.y != 0)
        {
            EventBus.Raise<OnStartClimb>();
            _moveHandler.MoveLinear(new Vector3(0, inputDelta.y, 0) * Time.deltaTime * _playerConfig.climbSpeed);
        }
        else
            EventBus.Raise<OnStopClimb>();

        _moveHandler.Update();
    }

    private void StopClimb(InputAction.CallbackContext callbackContext)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
}
