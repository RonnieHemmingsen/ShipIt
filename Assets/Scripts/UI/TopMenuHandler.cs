using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TopMenuHandler : MonoBehaviour {

    [SerializeField]
    private CanvasGroup _facebookMenu;
    [SerializeField]
    private CanvasGroup _globalLeaderboardPanel;
    [SerializeField]
    private CanvasGroup _friendsLeaderboardPanel;
    [SerializeField]
    private Button _globalButton;
    [SerializeField]
    private Button _friendsButton;

    private string _activeLeaderboard;


	// Use this for initialization
	void Start () {

        _activeLeaderboard = _globalLeaderboardPanel.name;
        if(PlayerData.instance.HasUserLoggedIn)
        {
            Utilities.MenuOff(_facebookMenu);
            Utilities.MenuOff(_friendsLeaderboardPanel);
            Utilities.MenuOn(_globalLeaderboardPanel);
        }
        else
        {
            Utilities.MenuOn(_facebookMenu);
            Utilities.MenuOff(_friendsLeaderboardPanel);
            Utilities.MenuOff(_globalLeaderboardPanel);
        }
	}

    void OnEnable()
    {
        EventManager.StartListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, EnableFacebookPanel);
        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, DisableFacebookPanel);
    }

    void OnDisable()
    {
        EventManager.StopListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, EnableFacebookPanel);
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, DisableFacebookPanel);
    }

	
    public void SwitchToFriends()
    {
        //if already on friends, do nothing - user doubleclicking leads here.
        if (_activeLeaderboard == _friendsLeaderboardPanel.name)
        {
            return;
        }
        
        _activeLeaderboard = _friendsLeaderboardPanel.name;
        Utilities.MenuOff(_globalLeaderboardPanel);
        Utilities.MenuOn(_friendsLeaderboardPanel);
    }

    public void SwitchToGlobal()
    {
        if (_activeLeaderboard == _globalLeaderboardPanel.name)
        {
            return;
        }
        _activeLeaderboard = _globalLeaderboardPanel.name;

        Utilities.MenuOn(_globalLeaderboardPanel);
        Utilities.MenuOff(_friendsLeaderboardPanel);
    }


    private void EnableFacebookPanel()
    {
        TurnButtonsOff();
        Utilities.MenuOff(_friendsLeaderboardPanel);
        Utilities.MenuOff(_globalLeaderboardPanel);
        Utilities.MenuOn(_facebookMenu);
    }

    private void DisableFacebookPanel()
    {
        TurnButtonsOn();
        Utilities.MenuOff(_facebookMenu);
        Utilities.MenuOff(_friendsLeaderboardPanel);
        Utilities.MenuOn(_globalLeaderboardPanel);
    }

    private void TurnButtonsOff()
    {
        _friendsButton.interactable = false;
        _globalButton.interactable = false;
    }

    private void TurnButtonsOn()
    {
        _friendsButton.interactable = true;
        _globalButton.interactable = true;
    }
}
