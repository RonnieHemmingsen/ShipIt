using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenericButton : MonoBehaviour {

    [SerializeField]
    private string _event;

    private Button _me;
	// Use this for initialization
	void Awake () {
        _me = GetComponent<Button>();
	}
	
	void Start () {
        _me.onClick.AddListener(delegate {
            print(_event + " clicked");
            EventManager.TriggerEvent(_event);
        });
    }
}