using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LeaderBoardButton : MonoBehaviour {

    private Button _me;

	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>();
	}

    void Start()
    {
        _me.onClick.AddListener(delegate {
            EventManager.TriggerEvent(MenuStrings.LEADER_BOARD_MENU_PRESSED);
        });
    }
}
