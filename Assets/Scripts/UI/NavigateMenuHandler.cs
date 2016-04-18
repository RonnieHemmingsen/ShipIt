using UnityEngine;
using System.Collections;

public class NavigateMenuHandler : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	

	}
	
    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.LEADER_BOARD_MENU_PRESSED, LeaderBoardMenuPressed);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.LEADER_BOARD_MENU_PRESSED, LeaderBoardMenuPressed);
    }

    private void LeaderBoardMenuPressed()
    {
        
    }
}
