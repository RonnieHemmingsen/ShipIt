using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DestroyAllInstructionsButton : MonoBehaviour {

    private Button _me;

	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>();
	}
	
	void Start () {
        _me.onClick.AddListener(delegate {
            print("DestroyAllInstructions clicked");
            EventManager.TriggerEvent(MenuStrings.ENABLE_DESTROYALL_INSTRUCTIONS);
        });
	}
}
