using UnityEngine;

public class PauseMenuView : MenuView
{
    private EventBus EventBus => ServiceProvider.Instance.GetService<EventBus>();

    public override void OnEnter()
    {
        base.OnEnter();
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public override void OnExit()
    {
        base.OnExit();
        Time.timeScale = 1.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventBus.Raise<OnPlayerUnpauseAccepted>();
    }
}
