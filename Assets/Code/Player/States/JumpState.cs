using UnityEngine;

public class JumpState : PlayerState
{
    private MovementHandler _movementHandler;
    private JumpHandler _jumpHandler;

    public JumpState(PlayerController p) : base(p)
    {
        _handlers.Add(_movementHandler = new MovementHandler(p));
        _handlers.Add(_jumpHandler = new JumpHandler(p));
    }

    public override void Update()
    {
    }
}
