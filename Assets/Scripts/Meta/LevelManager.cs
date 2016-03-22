using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance = null;

    private bool _wasStartMenuUpdated;
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
        //GameSparksHandler.AuthenticateUser("FB Player", "pw");
    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.BOOT_GAME, ToggleStartMenu);
        EventManager.StartListening(GameSettings.GAME_OVER, ResetGame);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.BOOT_GAME, ToggleStartMenu);
        EventManager.StopListening(GameSettings.GAME_OVER, ResetGame);
    }

    void Update()
    {
        if(!_wasStartMenuUpdated && _isStartMenuActive)
        {
            if (!SceneManager.SetActiveScene(SceneManager.GetSceneByName("Start Menu")))
            {
                _wasStartMenuUpdated = false;
            }else
            {
                _wasStartMenuUpdated = true;
            }
        }
    }


    private void ToggleStartMenu()
    {
        _isStartMenuActive = true;
        _wasStartMenuUpdated = false;
    }

    private void ResetGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
        SceneManager.LoadScene("Start Menu", LoadSceneMode.Additive);

        ToggleStartMenu();

    }
        

}
