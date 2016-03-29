using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OnlineButton : MonoBehaviour {

    private GameSparksHandler _GSHandler;
    private Button _me;

    void Awake()
    {
        _GSHandler = FindObjectOfType<GameSparksHandler>();
        _me = GetComponent<Button>();
    }

	// Use this for initialization
	void Start () {
        _me.onClick.AddListener(delegate {
            _GSHandler.StartFacebookLogin();
            print("her");
        });



	
	}
	
}
