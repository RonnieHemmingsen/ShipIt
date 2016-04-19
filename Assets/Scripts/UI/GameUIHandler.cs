using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIHandler : MonoBehaviour {
    [SerializeField]
    private CanvasGroup _gameOverMenu;
    [SerializeField]
    private CanvasGroup _pauseMenu;
    [SerializeField]
    private CanvasGroup _introScreen;
    [SerializeField]
    private Text _distanceText;
    [SerializeField]
    private Text _speedText;
    [SerializeField]
    private Text _boltCount;
    [SerializeField]
    private Text _currentFGScore;

    private GameManager _GM;
    private CanvasGroup _gameUI;

    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();
        _gameUI = GetComponent<CanvasGroup>();

    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, DisableSelf);
        EventManager.StartListening(MenuStrings.ENABLE_GAMEOVER_MENU, EnableGameOverMenu);
        EventManager.StartListening(MenuStrings.DISABLE_GAMEOVER_MENU, DisableGameOverMenu);
        EventManager.StartListening(MenuStrings.ENABLE_PAUSE_MENU, EnablePauseMenu);
        EventManager.StartListening(MenuStrings.DISABLE_PAUSE_MENU, DisablePauseMenu);

        EventManager.StartListening(MenuStrings.ENABLE_INTRO_SCREEN, EnableIntro);
        EventManager.StartListening(MenuStrings.DISABLE_INTRO_SCREEN, DisableIntro);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, DisableSelf);
        EventManager.StopListening(MenuStrings.ENABLE_GAMEOVER_MENU, EnableGameOverMenu);
        EventManager.StopListening(MenuStrings.DISABLE_GAMEOVER_MENU, DisableGameOverMenu);
        EventManager.StopListening(MenuStrings.ENABLE_PAUSE_MENU, EnablePauseMenu);
        EventManager.StopListening(MenuStrings.DISABLE_PAUSE_MENU, DisablePauseMenu);

        EventManager.StartListening(MenuStrings.ENABLE_INTRO_SCREEN, EnableIntro);
        EventManager.StartListening(MenuStrings.DISABLE_INTRO_SCREEN, DisableIntro);
    }

	// Use this for initialization
	void Start () {
	
        Utilities.MenuOff(_gameOverMenu);
        Utilities.MenuOff(_pauseMenu);
        Utilities.MenuOff(_introScreen);

        _distanceText.text = "Distance: 0";
        _currentFGScore.text = "0";
        _boltCount.text = "";

        EventManager.TriggerEvent(GameSettings.GAME_UI_EXISTS);
	}

	
	// Update is called once per frame
	void Update () {
        
        _distanceText.text = PlayerData.instance.Scores.lastTravelScore.ToString("F1");
        _currentFGScore.text = PlayerData.instance.Scores.lastCoinScore.ToString(); 
        _speedText.text = _GM.GameSpeed.ToString();
        _boltCount.text = _GM.CurrentNumberOfBolts.ToString();
	}

    private void EnableGameOverMenu()
    {
        Utilities.MenuOn(_gameOverMenu);
    }

    private void DisableGameOverMenu()
    {
        Utilities.MenuOff(_gameOverMenu);
    }

    private void EnablePauseMenu()
    {
        Utilities.MenuOn(_pauseMenu);
        Utilities.Pause();
    }

    private void DisablePauseMenu()
    {
        Utilities.UnPause();
        Utilities.MenuOff(_pauseMenu);
    }

    private void EnableIntro()
    {
        Utilities.MenuOn(_introScreen);
    }

    private void DisableIntro()
    {
        Utilities.MenuOff(_introScreen);
    }

    private void EnableSelf()
    {
        Utilities.MenuOn(_gameUI);
    }

    private void DisableSelf()
    {
        Utilities.MenuOff(_gameUI);
    }


}
