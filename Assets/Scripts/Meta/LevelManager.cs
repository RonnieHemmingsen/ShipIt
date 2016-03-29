using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance = null;

    private bool _hasActiveSceneUpdated;
    private bool _isStartMenuActive;

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
        EventManager.StartListening(GameSettings.UPDATE_ACTIVE_SCENE, ToggleStartMenu);
        EventManager.StartListening(GameSettings.GAME_OVER, ResetGame);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.UPDATE_ACTIVE_SCENE, ToggleStartMenu);
        EventManager.StopListening(GameSettings.GAME_OVER, ResetGame);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name != GameSettings.BOOT_SCENE)
        {
            if(!_hasActiveSceneUpdated && _isStartMenuActive)
            {
                print(SceneManager.GetActiveScene().name);
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSettings.MAIN_MENU_SCENE));
                print(SceneManager.GetActiveScene().name);
                _hasActiveSceneUpdated = true;
                
            }
            if(!_hasActiveSceneUpdated && !_isStartMenuActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(GameSettings.GAME_SCENE));
                _hasActiveSceneUpdated = true;
            }
        }
    }

    private void ToggleStartMenu()
    {
        _isStartMenuActive = !_isStartMenuActive;
        _hasActiveSceneUpdated = false;
    }


    private void ResetGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        SceneManager.LoadScene("Start Menu", LoadSceneMode.Additive);

        ToggleStartMenu();

    }
        

}
