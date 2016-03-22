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
    private Button _toMainMenu;
    [SerializeField]
    private Button _payWithCoins;
    [SerializeField]
    private Button _payWithAds;

    private LevelManager _lvlMan;
    private GameManager _GM;
    private PlayerData _data;
    private bool _survivesDeath;

    void Awake()
    {
        _lvlMan = FindObjectOfType<LevelManager>();
        _GM = FindObjectOfType<GameManager>();
        _data = FindObjectOfType<PlayerData>();
    }

	// Use this for initialization
	void Start () {
        
        _payWithCoins.onClick.AddListener(delegate {

            _survivesDeath = true;
            EventManager.TriggerIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, GameSettings.COST_OF_DEATH); 
            EventManager.TriggerEvent(GameSettings.START_GAME);
            EventManager.TriggerEvent(EventStrings.GET_REKT);
            Utilities.UnPause();
            
        });

        _payWithAds.onClick.AddListener(delegate {
            //TODO; Implement adds
            ShowAd();
            print("Show ad");
            _survivesDeath = true;

        });

        DisableMenu();
	}

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.ENABLE_GAMEOVER_MENU, EnableMenu);
        EventManager.StartListening(EventStrings.DISABLE_GAMEOVER_MENU, DisableMenu);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.ENABLE_GAMEOVER_MENU, EnableMenu);
        EventManager.StopListening(EventStrings.DISABLE_GAMEOVER_MENU, DisableMenu);
    }


    private void EnableMenu()
    {
        _toMainMenu.gameObject.SetActive(true);
        _payWithAds.gameObject.SetActive(true);
        _payWithCoins.gameObject.SetActive(true); 

        StartCoroutine(WaitForPermaDeath());

        if(GameSettings.COST_OF_DEATH > _data.GlobalCoinScore || _GM.PlayerDeathCount > 1)
        {
            _payWithCoins.interactable = false;
            _payWithAds.interactable = false;

        }

        Utilities.Pause();
    }

    private void DisableMenu()
    {
        _toMainMenu.gameObject.SetActive(false);
        _payWithAds.gameObject.SetActive(false);
        _payWithCoins.gameObject.SetActive(false);
    }
	

    private void ShowAd()
    {
        if(Advertisement.IsReady())
        {
            Advertisement.Show();
        }
    }

    private IEnumerator WaitForPermaDeath()
    {
        float time = Time.unscaledDeltaTime;
        float endTime = time + _GM.TimeUntilPermaDeath;

        do {
            time += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        } while (time < endTime);

        if(!_survivesDeath)
        {
            Utilities.UnPause();
            EventManager.TriggerEvent(GameSettings.GAME_OVER);
            EventManager.TriggerEvent(GameSettings.SAVE_DATA);  
        }

    }

    private void PostCoinScore()
    {

//        new LogEventRequest_ADD_COL().Send((response) => {
//            if (response.HasErrors) {
//                response.Errors.ToString();
//            }
//            else
//            {
//                print("saves created");
//            }
//        });
//



        //PersistentDataManager.SavePlayerData(GameSettings.COIN_SCORE, _GM.CurrentCoinScore);
    }

    private void PostTravelScore()
    {
//        if(GS.Authenticated)
//        {
//            new LogEventRequest_SCORE_EVENT()
//                .Set_TRAVEL_ATTR((long)_GM.CurrentTravelDistance)
//                .Set_SCORE_ATTR(_GM.CurrentCoinScore)
//                .Send((response) => 
//            {
//                if(response.HasErrors)
//                {
//                    print("score not posted");
//                }
//                else
//                {
//                    print("score posted");
//                }
//            });

//        }
    }
}
