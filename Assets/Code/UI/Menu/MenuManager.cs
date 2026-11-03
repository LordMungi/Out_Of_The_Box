using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private readonly Stack<IMenu> menuStack = new Stack<IMenu>();
    
    [SerializeField] private MenuView mainMenu;
    [SerializeField] private MenuView creditsMenu;
    [SerializeField] private MenuView pauseMenu;
    [SerializeField] private MenuView winMenu;
    
    public AK.Wwise.Event sfxClickPositive; 
    public AK.Wwise.Event sfxClickBack;
    private void Start()
    {
        if (mainMenu)
            mainMenu.Initialize(this);
        
        if (creditsMenu)
            creditsMenu.Initialize(this);
        
        if (pauseMenu)
            pauseMenu.Initialize(this);
        
        if (winMenu) 
            winMenu.Initialize(this);

        if (mainMenu != null)
            PushMenu(mainMenu, playSound: false);
    }
    public void PushMenu(MenuView newMenu, bool playSound = true)
    {
        if (newMenu == null) 
            return;

        if (menuStack.Count > 0)
            menuStack.Peek().OnPause();

        menuStack.Push(newMenu);
        newMenu.OnEnter();

        if (playSound && sfxClickPositive != null)
            sfxClickPositive.Post(gameObject);
    }

    public void PopMenu()
    {
        if (menuStack.Count == 0) 
            return;

        IMenu topMenu = menuStack.Pop();
        topMenu.OnExit();

        if (sfxClickBack != null)
            sfxClickBack.Post(gameObject);

        if (menuStack.Count > 0)
            menuStack.Peek().OnResume();
    }

    public void ClearStack()
    {
        while (menuStack.Count > 0)
            menuStack.Pop().OnExit();
    }

    public void OpenCredits() => PushMenu(creditsMenu);
    public void OpenPause() => PushMenu(pauseMenu);
    public void OpenWinScreen()
    {
        ClearStack();
        PushMenu(winMenu, playSound: false);
    }
}
