using UnityEngine;

public abstract class MenuView : MonoBehaviour, IMenu
{
    protected CanvasGroup canvasGroup;
    protected MenuManager manager;

    public bool hideUnderlyingMenu;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        hideUnderlyingMenu = true;
    }

    public void Initialize(MenuManager manager)
    {
        this.manager = manager;
        SetVisible(false);
    }

    public void OnEnter()
    {
        gameObject.SetActive(true);
        SetVisible(true);
    }

    public void OnExit()
    {
        gameObject.SetActive(false);
        SetVisible(false);
    }

    public void OnPause()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (hideUnderlyingMenu)
            canvasGroup.alpha = 0.0f;
    }

    public void OnResume()
    {
        SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1.0f : 0.0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}