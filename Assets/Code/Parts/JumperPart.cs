using UnityEngine;
using UnityEngine.InputSystem;

class JumperPart : RobotPart
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    protected override void SetActionPair()
    {
        type = Type.Jumper;

        actions.Add(new ActionPair(_inputActions.Player.Jump, ActionPair.Phase.Performed, SuperJump));
    }

    public void SuperJump(InputAction.CallbackContext callbackContext)
    {
        EventBus.Raise<OnSuperJumpRequest>();
    }
}