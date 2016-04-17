using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIHandler : MonoBehaviour {

    [SerializeField]
    private Text _distanceText;
    [SerializeField]
    private Text _speedText;
    [SerializeField]
    private Text _boltCount;
    [SerializeField]
    private Text _currentFGScore;

    private GameManager _GM;

    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();

    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, DisableSelf);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, DisableSelf);
    }

	// Use this for initialization
	void Start () {
	
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

    private void DisableSelf()
    {
        this.gameObject.SetActive(false);
    }


}
