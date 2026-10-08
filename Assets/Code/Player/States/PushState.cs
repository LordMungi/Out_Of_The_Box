using UnityEngine;
using UnityEngine.InputSystem;

public class PushState : PlayerState
{
    #region Fields
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    private PlayerInputActions _playerInput;
    private PlayerConfig _playerConfig;

    private MoveHandler _movementHandler;
    private InteractionsHandler _interactionsHandler;
    #endregion

    #region Initialization
    public PushState(ref PlayerData data)
    {
        _playerInput = data.input;
        _playerConfig = data.config;

        _handlers.Add(_movementHandler = data.handlers.movement);
        _handlers.Add(data.handlers.parts);
        _handlers.Add(_interactionsHandler = data.handlers.interactions);
    }

    public override void Enable()
    {
        _interactionsHandler.GrabNearestPushableObject(_movementHandler.Position);

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

            if(_interactionsHandler.CanPushObject(movementDelta))
                _movementHandler.MoveLinear(movementDelta);
        }
    }

    #region Input Callbacks
    private void OnObjectReleased(InputAction.CallbackContext context)
    {
        _nextState = BehaviourFSM.State.Idle;
        EventBus.Raise<OnPlayerStateChangeRequest>(_nextState);
    }
    #endregion
}
