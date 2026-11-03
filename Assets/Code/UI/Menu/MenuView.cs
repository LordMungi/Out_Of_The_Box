using UnityEngine;

public class MenuView : MonoBehaviour, IMenu
{
    protected CanvasGroup canvasGroup;
    protected MenuManager manager;

    public bool hideUnderlyingMenu;

    private void EnsureCanvasGroup()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();

            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    protected virtual void Awake()
    {
        hideUnderlyingMenu = true;
    }

    public void Initialize(MenuManager manager)
    {
        this.manager = manager;
        EnsureCanvasGroup();
        SetVisible(false);
        gameObject.SetActive(false);
    }

    public virtual void OnEnter()
    {
        gameObject.SetActive(true);
        SetVisible(true);
    }

    public virtual void OnExit()
    {
        gameObject.SetActive(false);
        SetVisible(false);
    }

    public virtual void OnPause()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        if (hideUnderlyingMenu)
        {
            canvasGroup.alpha = 0.0f;
            gameObject.SetActive(false);
        }
    }

    public virtual void OnResume()
    {
        gameObject.SetActive(true);
        SetVisible(true);
    }

    private void SetVisible(bool visible)
    {
        canvasGroup.alpha = visible ? 1.0f : 0.0f;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }
}