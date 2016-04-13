using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FBLogOutButton : MonoBehaviour {

    private OnlineManager _GSHandler;
    private Button _me;

    void Awake()
    {
        _GSHandler = FindObjectOfType<OnlineManager>();
        _me = GetComponent<Button>();
    }

    // Use this for initialization
    void Start () {
        _me.onClick.AddListener(delegate {
            _GSHandler.FBLogout();
            print("FB Logout button pressed");
        });
    }
}
