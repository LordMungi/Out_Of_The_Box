using UnityEngine;
using UnityEngine.InputSystem;

public class PushState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();
    #endregion

    #region Initialization
    public PushState(ref PlayerData data) : base(ref data) {}

    public override void Enable()
    {
        _interactionsHandler.GrabNearestPushableObject(_moveHandler.Position);

        _playerInput.Player.Release.performed += OnObjectReleased;
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        _playerInput.Player.Release.performed -= OnObjectReleased;

        _interactionsHandler.ReleasePushableObject();
    }

    #endregion
    public override void Update()
    {
        Vector2 inputDelta = _playerInput.Player.Move.ReadValue<Vector2>();

        if (inputDelta.y != 0)
        {
            Vector2 movementDelta = _interactionsHandler.DirectionVectorPushableObject * inputDelta.y * Time.deltaTime * _playerConfig.pushSpeed;
            
            if (inputDelta.y < 0)
                EventBus.Raise<OnPushBack>();
            else
                EventBus.Raise<OnPushForward>();

            if (_interactionsHandler.CanPushObject(movementDelta))
                _moveHandler.MoveLinear(movementDelta);
        }
        else
            EventBus.Raise<OnStopPushing>();
    }

    #region Input Callbacks
    private void OnObjectReleased(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
    #endregion
}
