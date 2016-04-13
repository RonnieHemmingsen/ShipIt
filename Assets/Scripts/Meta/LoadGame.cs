using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using GameSparks.Core;

public class LoadGame : MonoBehaviour {
    
    private OnlineManager _online;
    private OnlineStates _state;

    private bool _GSReady;

    private void Awake()
    {
        _online = FindObjectOfType<OnlineManager>();

    }
    void OnEnable()
    {

        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetConfirmUserIsLoggedInToGameSparks);
        EventManager.StartListening(OnlineStrings.FACEBOOK_NEW_USER, SetUserNotLoggedInToFacebook);
        EventManager.StartListening(OnlineStrings.FACEBOOK_INIT_DONE, SetUserIsLoggedInToFacebook);
        EventManager.StartListening(OnlineStrings.ONLINE_FALLTHROUGH, SetOnlineFallthroughState);
    }

    void OnDisable()
    {
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetConfirmUserIsLoggedInToGameSparks);
        EventManager.StopListening(OnlineStrings.FACEBOOK_INIT_DONE, SetUserIsLoggedInToFacebook);
        EventManager.StopListening(OnlineStrings.FACEBOOK_NEW_USER, SetUserNotLoggedInToFacebook);
        EventManager.StopListening(OnlineStrings.ONLINE_FALLTHROUGH, SetOnlineFallthroughState);
    }

    private void SetUserIsLoggedInToFacebook()
    {
        _state = OnlineStates.CheckGSStatus;
    }

    private void SetUserNotLoggedInToFacebook()
    {
        _state = OnlineStates.ConfirmUserOffline;
    }


    private void SetConfirmUserIsLoggedInToGameSparks()
    {
       
        _state = OnlineStates.ConfirmUserLoggedIn;
    }

    private void SetOnlineFallthroughState()
    {
        _state = OnlineStates.StartGame;
    }


	// Use this for initialization
    IEnumerator Start () {
        
        _state = OnlineStates.CheckInternetConnection;

        while (true)
        {
            switch (_state)
            {
                case OnlineStates.CheckInternetConnection:
                    CheckInternetConnection();
                    break;
                case OnlineStates.Init:
                    Init();
                    break;
                case OnlineStates.Idle:
                    Idle();
                    break;
                case OnlineStates.InitializeFB:
                    InitializeFB();
                    break;
                case OnlineStates.CheckGSStatus:
                    CheckGSStatus();
                    break;
                case OnlineStates.ConfirmUserLoggedIn:
                    ConfirmUserLoggedIn();
                    break;
                case OnlineStates.ConfirmUserOffline:
                    ConfirmUserOffline();
                    break;
                case OnlineStates.ConfirmFirstTimeUser:
                    ConfirmFirstTimeUser();
                    break;
                case OnlineStates.LoadPlayerData:
                    LoadPlayerData();
                    break;
                case OnlineStates.LoadPlayerName:
                    LoadPlayerName();
                    break;
                case OnlineStates.StartGame:
                    StartGame();
                    break;
                default:
                    break;
            }
            yield return 0;
        }

	}

    private void CheckInternetConnection()
    {
        StartCoroutine(Utilities.CheckInternetConnection((isConnected) => {
            if (isConnected) 
            {
                _state = OnlineStates.Init;
            }
            else
            {
                _state = OnlineStates.ConfirmUserOffline;
            }
        }));
        _state = OnlineStates.Idle;
    }

    private void Init()
    {
        print("Init");

        GS.GameSparksAvailable += (available) => {
            if (available) {
                print("GS available");
                _state = OnlineStates.InitializeFB;
            }
        };

        _state = OnlineStates.Idle;

    }

    private void InitializeFB()
    {
        print("Initialize FB");
        _online.InitializeFacebook();
        _state = OnlineStates.Idle;   
    }

    private void Idle()
    {
        //nothing
    }

    private void CheckGSStatus()
    {
        print("CheckGSStatus");
        _online.GameSparksLogin();
        _state = OnlineStates.Idle;
    }

    private void ConfirmUserLoggedIn()
    {
        print("ConfirmUserLoggedIn");
        PlayerData.instance.HasUserLoggedIn = true;
        _state = OnlineStates.LoadPlayerData;
    }

    private void ConfirmUserOffline()
    {
        print("ConfirmUserOffline");
        PlayerData.instance.HasUserLoggedIn = false;
        string id = PersistentDataManager.LoadGSUserId();

        _state = OnlineStates.Idle;

        if(string.IsNullOrEmpty(id))
        {
            StartCoroutine(PersistentDataManager.LoadPlayerData(id, (response) => {
                PlayerData.instance.Scores = response;
                print("Offline data exists - run with it");
                _state = OnlineStates.StartGame;
            }));    
        }
        else
        {
            _state = OnlineStates.ConfirmFirstTimeUser;
        }
    }

    private void ConfirmFirstTimeUser()
    {
        print("First time user, create dummy data, and save.");
        PlayerData.instance.Scores = PlayerData.instance.CreateEmptyData();
        PersistentDataManager.SavePlayerData(PlayerData.instance.Scores);
        _state = OnlineStates.StartGame;

    }

    private void LoadPlayerData()
    {
        print("LoadPlayerData");
        string id = PersistentDataManager.LoadGSUserId();
        StartCoroutine(PersistentDataManager.LoadPlayerData(id, (response) => {
                PlayerData.instance.Scores =  response;    

            _state = OnlineStates.LoadPlayerName;
        }));

        _state = OnlineStates.Idle;
    }

    private void LoadPlayerName()
    {
        print("Load Player Name");
        string id = PersistentDataManager.LoadGSUserId();
        StartCoroutine(PersistentDataManager.LoadPlayerName(id, (response) => {

            PlayerData.instance.Scores.userName = response;
            _state = OnlineStates.StartGame;
        }));

        _state = OnlineStates.Idle;
    }


    private void StartGame()
    {
        print("StartGame");
        SceneManager.LoadScene(2,LoadSceneMode.Single);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        EventManager.TriggerEvent(GameSettings.BOOT_GAME);
    }	
}

public enum OnlineStates
{
    CheckInternetConnection,
    Init,
    Idle,
    InitializeFB,
    CheckGSStatus,
    ConfirmUserLoggedIn,
    ConfirmUserOffline,
    ConfirmFirstTimeUser,
    LoadPlayerData,
    LoadPlayerName,
    StartGame

}
