using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Collections;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;
using GameSparks.Core;

public class EndGameMenuHandler : MonoBehaviour {

    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private Text _totalCoinsText;
    [SerializeField]
    private Button _payForLifeButton;
    [SerializeField]
    private Text _countDown;

    private GameManager _GM;
    private bool _survivesDeath;
    private bool _watchingAd;

    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();
    }

	// Use this for initialization
	void Start () {

        DisableMenu();
	}

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.ENABLE_GAMEOVER_MENU, EnableMenu);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.ENABLE_GAMEOVER_MENU, EnableMenu);
    }

    public void OnWatchAdClicked()
    {

        print("Show ad");
        _watchingAd = true;
        ShowAd();
    

    }

    public void OnPayForLifeClicked()
    {
        print("pay click lives");
        print("Coins before: " + PlayerData.instance.Scores.globalCoinScore);
        PlayerData.instance.Scores.globalCoinScore -= GameSettings.COST_OF_DEATH;
        print("Coins after: " + PlayerData.instance.Scores.globalCoinScore);
        SurviveDeath();
    }

    private void EnableMenu()
    {
        _menu.SetActive(true);
        _survivesDeath = false;

        _totalCoinsText.text = "(Have: " + PlayerData.instance.Scores.globalCoinScore.ToString() + ")";

        //house limit is one dueover!
        if(_GM.PlayerDeathCount > 1)
        {
            PermaDeath();

        }
        else
        {
            StartCoroutine(WaitForPermaDeath());
            Utilities.Pause();
        }

        if(PlayerData.instance.Scores.globalCoinScore < GameSettings.COST_OF_DEATH)
        {
            _payForLifeButton.interactable = false;   
        }
        else
        {
            _payForLifeButton.interactable = true; 
        }
    }

    private void DisableMenu()
    {
        _menu.SetActive(false);
    }
	

    private void ShowAd()
    {
        if(Advertisement.IsReady())
        {
            ShowOptions options = new ShowOptions();
            options.resultCallback = HandleShowResult;
            Advertisement.Show("", options);
        }
    }

    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                SurviveDeath();
                _watchingAd = false;
                print("Ad completed");
                break;
            case ShowResult.Skipped:
                PermaDeath();
                _watchingAd = false;
                print("Player skipped the ad");
                break;
            case ShowResult.Failed:
                PermaDeath();
                _watchingAd = false;
                print("The ad failed somehow: " + ShowResult.Failed.ToString());
                break;
            default:
                break;
        }
    }

    private void SurviveDeath()
    {            
        DisableMenu();
        Utilities.UnPause();
        EventManager.TriggerEvent(EventStrings.GET_REKT);
        _survivesDeath = true;
        StartCoroutine(WaitABit(1, "Restart"));

    }

    private void PermaDeath()
    {
        Utilities.UnPause();
        _GM.IsWaitingForNewGame = true;
        DisableMenu();
        print("Perma death");
        EventManager.TriggerEvent(EventStrings.GET_REKT);
        //vent på at alt er eksploderet færdig, før vi fortsætter
        StartCoroutine(WaitABit(1, "GameOver"));

    }

    private IEnumerator WaitABit(float timeToWait, string calledBy)
    {
        float time = 0;
        while (time <= timeToWait)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        print("Done waiting a bit");

        switch (calledBy) 
        {
            case "GameOver":
                GameOver();
                break;
            case "Restart":
                Restart();
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitForPermaDeath()
    {
        float time = Time.unscaledDeltaTime;
        float endTime = time + _GM.TimeUntilPermaDeath;

        do {
            time += Time.unscaledDeltaTime;
            _countDown.text = Mathf.RoundToInt(_GM.TimeUntilPermaDeath - time).ToString();

            yield return new WaitForEndOfFrame();
        } while (time < endTime || _watchingAd);

        print("Done waiting for death");

        if(!_survivesDeath)
        {
            PermaDeath();
        }
    }

    private void GameOver()
    {
        PersistentDataManager.SavePlayerData(PlayerData.instance.Scores);
        EventManager.TriggerEvent(GameSettings.GAME_OVER);
    }

    private void Restart()
    {
        
        EventManager.TriggerEvent(GameSettings.RESET_GAME);
    }
}
