using UnityEngine;
using UnityEngine.InputSystem;

class JumperPart : RobotPart
{
    protected override void SetActionPair()
    {
        actions.Add(new ActionPair(_inputActions.Player.Jump, ActionPair.Phase.Performed, SuperJump));
    }

    public void SuperJump(InputAction.CallbackContext callbackContext)
    {
        Debug.Log("SuperJump");
    }
}