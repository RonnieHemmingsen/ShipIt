using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartGameButton : MonoBehaviour {

    [SerializeField]
    private GameObject _mainMenu;
    [SerializeField]
    private GameObject _loadScreenUI;

    private LevelManager _lvlMan;
    private LoadScreen _loadScreen;
    private Button _me;
    // Use this for initialization

    void Awake()
    {
        _loadScreen = FindObjectOfType<LoadScreen>();
        _me = GetComponent<Button>();
    }

    void Start () 
    {

        _me.onClick.AddListener(delegate {
            //_lvlMan.LoadLevel(GameSettings.LOAD_LEVEL_GAME);
            _mainMenu.SetActive(false);
            _loadScreenUI.SetActive(true);
            _loadScreen.StartLoadSequence(GameSettings.LOAD_LEVEL_GAME);
        });
	}
	

}
