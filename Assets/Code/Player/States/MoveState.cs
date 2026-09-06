using UnityEngine;

public class MoveState : PlayerState
{
    private MovementHandler _movementHandler;

    public MoveState(PlayerController p) : base(p)
    {
        _handlers.Add(_movementHandler = new MovementHandler(p));
    }

    public override void Update()
    {

    }
}
