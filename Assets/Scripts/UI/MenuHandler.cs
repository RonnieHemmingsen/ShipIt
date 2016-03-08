using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
using System.Collections;
using GameSparks.Api.Requests;
using GameSparks.Api.Responses;

public class MenuHandler : MonoBehaviour {

    [SerializeField]
    private Button _toMainMenu;
    [SerializeField]
    private Button _payWithCoins;
    [SerializeField]
    private Button _payWithAds;

    private LevelManager _lvlMan;
    private GameManager _GM;

    void Awake()
    {
        _lvlMan = FindObjectOfType<LevelManager>();
        _GM = FindObjectOfType<GameManager>();
    }

	// Use this for initialization
	void Start () {
        
        _toMainMenu.onClick.AddListener(delegate {
            _lvlMan.LoadLevel(GameSettings.LOAD_LEVEL_MENU);
            Time.timeScale = 1;
        });

        _payWithCoins.onClick.AddListener(delegate {
            SceneManager.LoadScene(GameSettings.LOAD_LEVEL_GAME);
            Time.timeScale = 1;
        });

        _payWithAds.onClick.AddListener(delegate {
            //TODO; Implement adds
            ShowAd();
            print("Show ad");

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

        PostScore();

        Time.timeScale = 0;
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

    private void PostScore()
    {
        new LogEventRequest_SCORE_EVENT().Set_SCORE_ATTR(_GM.CoinScore).Send((response) => 
        {
            if(response.HasErrors)
            {
                print("score not posted");
            }
            else
            {
                print("score posted");
            }
        });
    }
}
