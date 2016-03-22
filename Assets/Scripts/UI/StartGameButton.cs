using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartGameButton : MonoBehaviour {

    [SerializeField]
    private GameObject _mainMenu;

    private LevelManager _lvlMan;
    private LoadScreen _loadScreen;
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
            _mainMenu.SetActive(false);
            EventManager.TriggerEvent(GameSettings.START_GAME);
            _lvlMan.IsStartMenuActive = false;
            Scene gamescene = SceneManager.GetSceneByName("Game");
            SceneManager.SetActiveScene(gamescene);


        });
	}
	

}
