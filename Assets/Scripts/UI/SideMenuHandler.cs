using UnityEngine;
using System.Collections;

public class SideMenuHandler : MonoBehaviour {
    
    [SerializeField]
    private CanvasGroup _profileMenu;
    [SerializeField]
    private CanvasGroup _creditsMenu;

    private string _activeMenu;

	// Use this for initialization
	void Start () {
        _activeMenu = _profileMenu.name;
        Utilities.MenuOff(_creditsMenu);
        Utilities.MenuOn(_profileMenu);
	}
	
    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, ResetForGameOver);
        EventManager.StartListening(MenuStrings.CREDITS_PRESSED, EnableCredits);
        EventManager.StartListening(MenuStrings.PROFILE_MENU_PRESSED, EnableProfile);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, ResetForGameOver);
        EventManager.StopListening(MenuStrings.CREDITS_PRESSED, EnableCredits);
        EventManager.StopListening(MenuStrings.PROFILE_MENU_PRESSED, EnableProfile);
    }

    private void ResetForGameOver()
    {
        EnableProfile();
    }

    private void EnableCredits()
    {
        //already active. All is well
        if(_activeMenu == _creditsMenu.name)
        {
            return;
        }

        //switch panels
        if(_activeMenu == _profileMenu.name)
        {
            _activeMenu = _creditsMenu.name;
            Utilities.MenuOff(_profileMenu);
            Utilities.MenuOn(_creditsMenu);
        }

    }

    private void EnableProfile()
    {
        if(_activeMenu == _profileMenu.name)
        {
            return;
        }

        if(_activeMenu == _creditsMenu.name)
        {
            _activeMenu = _profileMenu.name;
            Utilities.MenuOff(_creditsMenu);
            Utilities.MenuOn(_profileMenu);
        }
    }

}
