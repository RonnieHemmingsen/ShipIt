using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnlineButton : MonoBehaviour {
    
    private Button _me;

    void Awake()
    {
        _me = GetComponent<Button>();
    }

	// Use this for initialization
	void Start () {
        _me.onClick.AddListener(delegate {
            EventManager.TriggerEvent(OnlineStrings.ONLINE_BUTTON_PRESSED);
            print("Login button clicked");
        });



	
	}
	
}
