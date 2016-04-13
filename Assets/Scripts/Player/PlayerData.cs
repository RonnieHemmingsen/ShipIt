using UnityEngine;
using System;
using System.Collections;

public class PlayerData : MonoBehaviour {

    public static PlayerData instance;

    private bool _hasUserLoggedIn;
    private Data _scores;

    #region properties

    public Data Scores 
    {
        get { return _scores; }
        set { _scores = value; }
    }

    public bool HasUserLoggedIn
    {
        get { return _hasUserLoggedIn; }
        set { _hasUserLoggedIn = value; }
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

        _scores = new Data();
    }

        
    void OnEnable()
    {
        EventManager.StartListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StartListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);

    }

    void OnDisable()
    {
        EventManager.StopListeningForIntEvent(EventStrings.ADD_TO_GLOBAL_COINSCORE, AddToGlobalCoinScore);
        EventManager.StopListeningForIntEvent(EventStrings.SUBTRACT_FROM_GLOBAL_COINSCORE, SubtractFromGlobalScore);
    }

    public Data CreateEmptyData()
    {
        Data dat = new Data();
        dat.globalCoinScore = 0;
        dat.lastCoinScore = 0;
        dat.highestTravelScore = 0;
        dat.lastTravelScore = 0;
        dat.userName = "Commander";
        dat.timeStamp = DateTime.Now;

        return dat;
            
    }

    private void AddToGlobalCoinScore(int amount)
    {
        _scores.globalCoinScore += amount;
    }

    private void SubtractFromGlobalScore(int amount)
    {
        _scores.globalCoinScore -= amount;      
          
    }

        
}
