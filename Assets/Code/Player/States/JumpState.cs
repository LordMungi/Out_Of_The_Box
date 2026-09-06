using UnityEngine;

public class JumpState : PlayerState
{
    private MoveHandler _movementHandler;
    private JumpHandler _jumpHandler;

    public JumpState(ref PlayerData p) : base(ref p)
    {
        _handlers.Add(_movementHandler = new MoveHandler(ref p, p.config.jumpMoveSpeed));
        _handlers.Add(_jumpHandler = new JumpHandler(ref p));
    }

    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
    }

}
