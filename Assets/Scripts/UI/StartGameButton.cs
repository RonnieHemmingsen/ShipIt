using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGameButton : MonoBehaviour {

    private LevelManager _lvlMan;
    private Button _me;
    // Use this for initialization

    void Awake()
    {
        _lvlMan = FindObjectOfType<LevelManager>();
        _me = GetComponent<Button>();
    }

    void Start () 
    {

        _me.onClick.AddListener(delegate {
            _lvlMan.LoadLevel(GameSettings.LOAD_LEVEL_GAME);
        });
	}
	

}
