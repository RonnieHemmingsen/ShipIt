using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoggedInMenuHandler : MonoBehaviour {

    [SerializeField]
    private Text _userNameText;
    [SerializeField]
    private Text _coinScoreText;
    [SerializeField]
    private Text _travelScoreText;

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.UPDATE_LOGGED_IN_MENU, UpdateMenu);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.UPDATE_LOGGED_IN_MENU, UpdateMenu);
    }


    private void UpdateMenu()
    {
        print("updatemenu");
        _userNameText.text = PlayerData.instance.FBName;
        _coinScoreText.text = "Coin score: " + PlayerData.instance.GlobalCoinScore.ToString();
        _travelScoreText.text = "Travel score " + PlayerData.instance.HighestTravelScore.ToString();

    }



}
