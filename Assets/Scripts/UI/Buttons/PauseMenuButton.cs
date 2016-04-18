using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PauseMenuButton : MonoBehaviour {

    private Button _me;
    private bool _isActive;
	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>();
	}
	
	// Update is called once per frame
	void Start () {
        _me.onClick.AddListener(delegate {
            print("Pause game pressed");
            if(!_isActive)
            {
                print("Is Active: " + _isActive);
                EventManager.TriggerEvent(MenuStrings.ENABLE_PAUSE_MENU);
                _isActive = true;
            }
            else 
            {
                print("Is Active: " + _isActive);
                EventManager.TriggerEvent(MenuStrings.DISABLE_PAUSE_MENU);
                _isActive = false;
            }

        });
	
	}
}
