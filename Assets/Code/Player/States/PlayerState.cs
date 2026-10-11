using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected List<PlayerHandler> _handlers = new List<PlayerHandler>();
    protected BehaviourFSM.State _nextState;

    protected PlayerInputActions _playerInput;
    protected PlayerConfig _playerConfig;

    protected MoveHandler _moveHandler;
    protected PartHandler _partHandler;
    protected InteractionsHandler _interactionsHandler;
    protected CameraHandler _cameraHandler;

    public PlayerState(ref PlayerData data)
    {
        _playerInput = data.input;
        _playerConfig = data.config;

        _handlers.Add(_moveHandler = data.handlers.movement);
        _handlers.Add(_partHandler = data.handlers.parts);
        _handlers.Add(_interactionsHandler = data.handlers.interactions);
        _handlers.Add(_cameraHandler = data.handlers.camera);
    }

    public abstract void Update();

    public virtual void Enable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Enable();
        }
    }
    public virtual void Disable()
    {
        foreach (PlayerHandler handler in _handlers)
        {
            handler.Disable();
        }
    }
}
