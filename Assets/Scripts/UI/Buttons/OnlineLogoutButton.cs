using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnlineLogoutButton : MonoBehaviour {

    private Button _me;

    void Awake()
    {
        _me = GetComponent<Button>();
    }

    // Use this for initialization
    void Start () {
        _me.onClick.AddListener(delegate {
            EventManager.TriggerEvent(OnlineStrings.OFFLINE_BUTTON_PRESSED);
            print("Logout button clicked");
        });
    }
}
