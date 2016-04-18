using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance = null;

    private bool _hasActiveSceneUpdated;
    private bool _isStartMenuActive;
    private CanvasGroup _mainMenu;
    private CanvasGroup _gameUI;


    public bool IsStartMenuActive {
        get { return _isStartMenuActive; }
        set { _isStartMenuActive = value; }
    }

    void Awake()
    {
        if (instance != null) {
            Destroy (this.gameObject);
            print ("Dupe Levelmanager self-destructing!");
        } else 
        {
            print("we cool");
            instance = this;
            GameObject.DontDestroyOnLoad (this.gameObject);
        }
    }

    void Start()
    {
    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_UI_EXISTS, FindGameUI);
        EventManager.StartListening(GameSettings.MAIN_MENU_EXISTS, FindMainMenu);
        EventManager.StartListening(GameSettings.BOOT_GAME, BootGame);
        EventManager.StartListening(GameSettings.START_GAME, StartGame);
        EventManager.StartListening(GameSettings.GAME_OVER, GameOver);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_UI_EXISTS, FindGameUI);
        EventManager.StopListening(GameSettings.MAIN_MENU_EXISTS, FindMainMenu);
        EventManager.StopListening(GameSettings.BOOT_GAME, BootGame);
        EventManager.StopListening(GameSettings.START_GAME, StartGame);
        EventManager.StopListening(GameSettings.GAME_OVER, GameOver);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != GameSettings.BOOT_SCENE)
        {
            if(!_hasActiveSceneUpdated && _isStartMenuActive)
            {
                print(SceneManager.GetActiveScene().name);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSettings.MAIN_MENU_SCENE));
                //print(SceneManager.GetActiveScene().name);

                _hasActiveSceneUpdated = true;
                
            }
            if(!_hasActiveSceneUpdated && !_isStartMenuActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSettings.GAME_SCENE));

                _hasActiveSceneUpdated = true;
            }
        }
    }

    private void ToggleScenes()
    {
        _isStartMenuActive = !_isStartMenuActive;
        _hasActiveSceneUpdated = false;
    }

    private void BootGame()
    {
        ToggleScenes();
    }

    private void GameOver()
    {
        _gameUI.alpha = 0;
        _mainMenu.alpha = 1;
        ToggleScenes();
    }

    private void StartGame()
    {
        _gameUI.alpha = 1;
        _mainMenu.alpha = 0;
        ToggleScenes();
    }

    private void FindMainMenu()
    {
        _mainMenu = GameObject.Find("Main Menu").GetComponent<CanvasGroup>();   
        //print(_mainMenu.name);
    }

    private void FindGameUI()
    {
        _gameUI = GameObject.Find("GameUI").GetComponent<CanvasGroup>();
        _gameUI.alpha = 0;
        //print(_gameUI.name);
    }
        

}
