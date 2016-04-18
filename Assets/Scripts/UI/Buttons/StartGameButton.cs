using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour {
    
    private Button _me;
    // Use this for initialization

    void Awake()
    {
        _me = GetComponent<Button>();
    }

    void Start () 
    {
        _me.onClick.AddListener(delegate {
            print("Press Play On Tape");
            EventManager.TriggerEvent(MenuStrings.CLEAR_MENUS_FOR_GAME);

        });
    }

}
