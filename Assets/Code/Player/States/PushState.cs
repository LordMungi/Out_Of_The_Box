using UnityEngine;

public class PushState : PlayerState
{
    #region Fields
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
        _interactionsHandler.GrabNearestPushableObject();
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
    }

    #endregion
    public override void Update()
    {
        Vector2 delta = _playerInput.Player.Move.ReadValue<Vector2>();

        if (delta.y != 0)
        {
            _interactionsHandler.PushObject(delta.y, _playerConfig.pushSpeed);
            _movementHandler.SetPosition(_interactionsHandler.GetOffsetedPositionFromPushableObejct());
        }
    }
}
