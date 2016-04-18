using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProfileMenuButton : MonoBehaviour {

    private Button _me;

	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>(); 
	}
	
	// Update is called once per frame
	void Start () {
        _me.onClick.AddListener(delegate {
            print("Pressed Profile Menu");
            EventManager.TriggerEvent(MenuStrings.PROFILE_MENU_PRESSED);
        });
	
	}
}
