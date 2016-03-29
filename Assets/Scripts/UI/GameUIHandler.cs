using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUIHandler : MonoBehaviour {

    [SerializeField]
    private Text _distanceText;
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _killText;
    [SerializeField]
    private Text _timePlayedText;
    [SerializeField]
    private Text _speedText;
    [SerializeField]
    private Text _boltCount;

    private GameManager _GM;
    private PlayerController _PC;


    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();
        _PC = FindObjectOfType<PlayerController>();
    }

	// Use this for initialization
	void Start () {
	
        _distanceText.text = "Distance: 0";
        _scoreText.text = "0";
        _boltCount.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        _timePlayedText.text = Mathf.RoundToInt(Time.time).ToString();

        _distanceText.text = _GM.CurrentTravelDistance.ToString("F1");
        _scoreText.text = _GM.CurrentCoinScore.ToString(); 
        _speedText.text = _GM.GameSpeed.ToString();
        _boltCount.text = _PC.CurrentNumberOfBolts.ToString();
	}
}
