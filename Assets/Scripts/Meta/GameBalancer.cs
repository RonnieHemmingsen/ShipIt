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
    private float _timeBeforeLaserEnemiesAppear;
    [SerializeField]
    private float _timeBeforeBulletEnemiesAppear;
    [SerializeField]
    private int _maxNumberOfLaserEnemies;
    [SerializeField]
    private int _maxNumberOfBulletEnemies;

    private GameManager _GM;
    private float _deltaTimeIncrement;

    #region Properties
    public int MaxNumberOfLaserEnemies
    {
        get { return _maxNumberOfLaserEnemies; }
        set { _maxNumberOfLaserEnemies = value; }
    }

    public int MaxNumberOfBulletEnemies
    {
        get { return _maxNumberOfBulletEnemies; }
        set { _maxNumberOfBulletEnemies = value; }
    }
    #endregion
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

        if(Time.time > _timeBeforeLaserEnemiesAppear)
        {
            //print("Enemies Timestamp: " + Time.deltaTime);
            EventManager.TriggerEvent(EventStrings.TOGGLE_LASER_ENEMY_SPAWNING);
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
