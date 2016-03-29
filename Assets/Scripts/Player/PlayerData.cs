using UnityEngine;
using System;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    private GameManager _GM;
    private GameSparksHandler _GSHandler;
    private DateTime _timeStamp;
    private int _globalCoinScore;
    private float _lastTravelScore;
    private float _highestTravelScore;
    private string _GSUserId;
    private string _FBUserId;
    private string _FBName;
    private bool _isLoggedInToFacebook;
    private bool _isLoggedInToGameSparks;
    private bool _hasRetrievedData;
    private bool _hasInternetConnection;
    private bool _isUserLoggedIn;

    #region properties
    public DateTime TimeStamp
    {
        get { return _timeStamp; }
    }

    public int GlobalCoinScore
    {
        get { return _globalCoinScore; }
    }

    public float HighestTravelScore
    {
        get { return _highestTravelScore; }
    }

    public string GSUserId
    {
        get { return _GSUserId; }
        set { _GSUserId = value; }
    }

    public string FBUserId
    {
        get { return _FBUserId; }
        set { _FBUserId = value; }
    }

    public string FBName
    {
        get { return _FBName; }
        set { _FBName = value; }
    }

    public bool IsLoggedInToFacebook
    {
        get { return _isLoggedInToFacebook; }
        set { _isLoggedInToFacebook = value; }
    }

    public bool IsLoggedInToGameSparks
    {
        get { return _isLoggedInToGameSparks; }
        set { _isLoggedInToGameSparks = value; }
    }

    #endregion

    void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }

        _GSHandler = FindObjectOfType<GameSparksHandler>();
    }

        
    void OnEnable()
    {
        EventManager.StartListening(GameSettings.SAVE_DATA, SaveData);
        EventManager.StartListening(GameSettings.GAME_OVER, ResetForNewGame);

        //PlayerData Exists before GameManager, hence this piece of stupid code.
        EventManager.StartListening(EventStrings.GET_GAME_MANAGER, GetGameManager);

        EventManager.StartListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StartListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);

    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.SAVE_DATA, SaveData);
        EventManager.StopListening(GameSettings.GAME_OVER, ResetForNewGame);
        EventManager.StopListening(EventStrings.GET_GAME_MANAGER, GetGameManager);

        EventManager.StopListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StopListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);
    }


    public IEnumerator CheckIfUserIsLoggedIn(Action<bool> callback)
    {
        float time = 0;
        while (time < 0.5f)
        {
            time += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        _isLoggedInToFacebook = _GSHandler.CheckFacebookAvailability() ? true : false;
        _isLoggedInToGameSparks = _GSHandler.CheckGSAvailability() ? true : false;

        print("FB logged in: " + _isLoggedInToFacebook);
        print("GS logged in: " + _isLoggedInToGameSparks);

        if(_isLoggedInToFacebook && _isLoggedInToGameSparks)
        {
            _isUserLoggedIn = true;
        }
        else
        {
            _isUserLoggedIn = false;
        }
        print("Is User logged in: " + _isUserLoggedIn);
        callback(_isUserLoggedIn);
    }

    private void GetGameManager()
    {
        _GM = FindObjectOfType<GameManager>(); 
    }

    private void AddToGlobalCoinScore(int amount)
    {
        _globalCoinScore += amount;
    }

    private void SubtractFromGlobalScore(int amount)
    {
        _globalCoinScore -= amount;      
          
    }


    private void SaveData()
    {
        if(_GM == null) return;


        if(IsNewTravelScoreHigher(_highestTravelScore, _GM.CurrentTravelDistance))
        {
            _highestTravelScore = _GM.CurrentTravelDistance;
        }
        PersistentDataManager.SavePlayerData(_globalCoinScore, _highestTravelScore, _lastTravelScore);
    }

    public IEnumerator LoadData(Action<bool> callback)
    {
        StartCoroutine(PersistentDataManager.LoadPlayerData(GSUserId, (response) => {
            print(_timeStamp.ToString());
            _timeStamp = new DateTime();
            _timeStamp = response.timeStamp;
            _globalCoinScore = response.globalCoinScore;
            _highestTravelScore = response.highestTravelScore;
            _lastTravelScore = response.lastTravelScore;

            print("Data loaded: " + _timeStamp);
            _hasRetrievedData = true;

        }));

        while(!_hasRetrievedData)
        {
            yield return new WaitForEndOfFrame();
        }

        callback(true);
    }

//    private IEnumerator ForFucksSake()
//    {
//        bool hasFinished = false;
//
//        StartCoroutine(PersistentDataManager.LoadPlayerData(UserId, (response) => {
//            _timeStamp = response.timeStamp;
//            _globalCoinScore = response.globalCoinScore;
//            _highestTravelScore = response.highestTravelScore;
//            _lastTravelScore = response.lastTravelScore;
//
//            print("Data loaded: " + _timeStamp);
//            hasFinished = true;
//        }));
//
//        while(!hasFinished)
//        {
//            
//            yield return new WaitForEndOfFrame();
//        }
//
//
//    }
//
    private void LoadGlobalLeaderBoard()
    {}

    private void LoadFriendLeaderBoard()
    {}

    private bool IsNewTravelScoreHigher(float prevScore, float thisScore)
    {
        return (thisScore > prevScore) ? true : false;
    }

    private void ResetForNewGame()
    {
        
        _hasRetrievedData = false;

    }

    //Not currently in use. Men smart! :P
    private IEnumerator CheckInternetConnection(Action<bool> callback)
    {
        WWW www = new WWW(GameSettings.URL_CHECK);
        yield return www;

        if(www.error != null)
        {
            callback(false);
        }
        else
        {
            callback(true);
        }
    }
        
}
