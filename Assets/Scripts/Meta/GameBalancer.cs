using UnityEngine;
using System.Collections;

public class GameBalancer : MonoBehaviour {

    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _speedIncrementValue;
    [SerializeField]
    private float _timeBetweenSpeedIncrements;
    [SerializeField]
    private float _timeBeforeEnemiesAppear;
    [SerializeField]
    private int _maxNumberOfEnemies;

    private GameManager _GM;
    private float _deltaTimeIncrement;

    public int MaxNumberOfEnemies
    {
        get { return _maxNumberOfEnemies; }
        set { _maxNumberOfEnemies = value; }
    }

    void Awake()
    {
        _GM = GetComponent<GameManager>();   
    }

	// Use this for initialization
	void Start () {
        _GM.CanSpawnAsteroids = true;   
	}
	
	// Update is called once per frame
	void Update () {
        _deltaTimeIncrement += Time.deltaTime;

        if(_deltaTimeIncrement > _timeBetweenSpeedIncrements && 
            _GM.GameSpeed >= _maxSpeed)
        {
            _deltaTimeIncrement = 0;
            EventManager.TriggerEvent(EventStrings.SPEED_INCREASE);
            //print("Speed Timestamp: " + Time.deltaTime);
        }

        if(Time.time > _timeBeforeEnemiesAppear)
        {
            //print("Enemies Timestamp: " + Time.deltaTime);
            EventManager.TriggerEvent(EventStrings.TOGGLE_ENEMY_SPAWNING);
        }

        AsteroidToggle();

	}

    private void AsteroidToggle()
    {
        float rand = Random.value;
        if(rand <= 0.01f)
        {
            //EventManager.TriggerEvent(EventStrings.TOGGLE_ASTEROID_SPAWNING);
            //print("AsteroidToggle - " + rand);
            
        }

    }
}
