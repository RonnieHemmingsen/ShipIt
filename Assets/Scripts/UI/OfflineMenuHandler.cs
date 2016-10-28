using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OfflineMenuHandler : MonoBehaviour {

    [SerializeField]
    private Text _userNameText;
    [SerializeField]
    private Text _coinHeaderText;
    [SerializeField]
    private Text _totalCoinScoreText;
    [SerializeField]
    private Text _lastCoinScoreText;
    [SerializeField]
    private Text _traveHeaderText;
    [SerializeField]
    private Text _lastTravelScoreText;
    [SerializeField]
    private Text _bestTravelScoreText;

    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.UPDATE_OFFLINE_MENU, UpdateMenu);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.UPDATE_OFFLINE_MENU, UpdateMenu);
    }

    private void UpdateMenu()
    {
        print("update offline menu");
        _userNameText.text = PlayerData.instance.Scores.userName;

        _traveHeaderText.text = TextStrings.LIGHTYEARS_TRAVELLED;
        _lastTravelScoreText.text = TextStrings.LAST_TRIP_LENGTH + PlayerData.instance.Scores.lastTravelScore.ToString("F1");
        _bestTravelScoreText.text = TextStrings.BEST_TRIP_LENGTH + PlayerData.instance.Scores.highestTravelScore.ToString("F1");

        _coinHeaderText.text = TextStrings.FUTURE_GOLD_SCORE;
        _lastCoinScoreText.text = TextStrings.FG_LAST + "\n" + PlayerData.instance.Scores.lastCoinScore.ToString();
        _totalCoinScoreText.text = TextStrings.FG_TOTAL + "\n" + PlayerData.instance.Scores.globalCoinScore.ToString();


    }


}
