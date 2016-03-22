using UnityEngine;
using System;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    private GameManager _GM;
    private DateTime _timeStamp;
    private int _globalCoinScore;
    private float _lastTravelScore;
    private float _highestTravelScore;
    private string _userId;

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

    public string UserId
    {
        get { return _userId; }
        set { _userId = value; }
    }

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


    }


    void OnEnable()
    {
        EventManager.StartListening(GameSettings.SAVE_DATA, SaveData);
        EventManager.StartListening(GameSettings.LOAD_DATA, LoadData);

        EventManager.StartListening(EventStrings.GET_GAME_MANAGER, GetGameManager);

        EventManager.StartListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StartListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.SAVE_DATA, SaveData);
        EventManager.StopListening(GameSettings.LOAD_DATA, LoadData);

        EventManager.StopListening(EventStrings.GET_GAME_MANAGER, GetGameManager);

        EventManager.StopListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StopListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);
    }

	// Use this for initialization
	void Start () {
        
	}

    void Update()
    {
        //print("Player data: " +_timeStamp);
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
        //TODO: Implement safety when doing the shop.
 
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

    private void LoadData()
    {
        StartCoroutine(PersistentDataManager.LoadPlayerData(UserId, (response) => {
            _timeStamp = response.timeStamp;
            _globalCoinScore = response.globalCoinScore;
            _highestTravelScore = response.highestTravelScore;
            _lastTravelScore = response.lastTravelScore;

            print("Data loaded: " + _timeStamp);
           
        }));
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
        
}
