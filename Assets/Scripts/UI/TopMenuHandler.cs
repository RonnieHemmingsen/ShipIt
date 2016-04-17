using UnityEngine;
using System.Collections;

public class TopMenuHandler : MonoBehaviour {

    [SerializeField]
    private CanvasGroup _globalLeaderboardPanel;
    [SerializeField]
    private CanvasGroup _friendsLeaderboardPanel;


    private string _activeLeaderboard;


	// Use this for initialization
	void Start () {
        _activeLeaderboard = _globalLeaderboardPanel.name;
        _globalLeaderboardPanel.alpha = 1;
        _friendsLeaderboardPanel.alpha = 0;
	}
	
    public void SwitchToFriends()
    {
        //if already on friends, do nothing - user doubleclicking leads here.
        if (_activeLeaderboard == _friendsLeaderboardPanel.name)
        {
            return;
        }
        
        _activeLeaderboard = _friendsLeaderboardPanel.name;
        _globalLeaderboardPanel.alpha = 0;
        _friendsLeaderboardPanel.alpha = 1;

    }

    public void SwitchToGlobal()
    {
        if (_activeLeaderboard == _globalLeaderboardPanel.name)
        {
            return;
        }
        _activeLeaderboard = _globalLeaderboardPanel.name;
        _globalLeaderboardPanel.alpha = 1;
        _friendsLeaderboardPanel.alpha = 0;
    }

}
