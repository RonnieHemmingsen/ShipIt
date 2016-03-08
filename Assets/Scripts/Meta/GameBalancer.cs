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
    private float _timeBetweenDifficultyIncrements;
    [SerializeField]
    private float _timeBetweenToggleIncrements;
    [SerializeField]
    private float _timeBeforeAsteroidsAppear;
    [SerializeField]
    private float _timeBeforeLaserEnemiesAppear;
    [SerializeField]
    private float _timeBeforeBulletEnemiesAppear;
    [SerializeField]
    private int _maxNumberOfLaserEnemies;
    [SerializeField]
    private int _maxNumberOfBulletEnemies;
    [SerializeField]
    private int _maxNumberOfAsteroids;
    [SerializeField]
    private float _timeBeforeTokensCanAppear;
    [SerializeField]
    private float _chanceToSpawnToken = 0.05f;
    [SerializeField]
    private float _chanceToSpawnBulletEnemy = 0.2f;
    [SerializeField]
    private float _chanceToSpawnLaserEnemy = 0.02f;
    [SerializeField]
    private float _chanceToSpawnAsteroid = 1f;

    private GameManager _GM;
    private int _difficultySetting;
    private float _deltaTimeIncrement;
    private float _deltaDifficultyIncrement;
    private float _deltaToggleAllIncrement;
    private bool _canSpawnTokens;
    private bool _canSpawnLaserEnemies;
    private bool _canSpawnBulletEnemies;
    private bool _canSpawnAsteroids;
    private bool _isNothing;


    #region Properties

    public int MaxNumberOfAsteroids
    {
        get { return _maxNumberOfAsteroids; }
        set { _maxNumberOfAsteroids = value; }
    }

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

    public float TimeBeforeAsteroidsCanAppear
    {
        get { return _timeBeforeAsteroidsAppear; }
        set { _timeBeforeAsteroidsAppear = value; }
    }

    public float TimeBeforeLaserEnemiesCanAppear
    {
        get { return _timeBeforeLaserEnemiesAppear; }
        set { _timeBeforeLaserEnemiesAppear = value; }
    }

    public float TimeBeforeBulletEnemiesCanAppear
    {
        get { return _timeBeforeBulletEnemiesAppear; }
        set { _timeBeforeBulletEnemiesAppear = value; }
    }
        
    public bool CanSpawnTokens
    {
        get { return _canSpawnTokens; }
        set { _canSpawnTokens = value; }
    }

    public bool CanSpawnLaserEnemies
    {
        get { return _canSpawnLaserEnemies; }
        set { _canSpawnLaserEnemies = value; }
    }

    public bool CanSpawnBulletEnemies
    {
        get { return _canSpawnBulletEnemies; }
        set { _canSpawnBulletEnemies = value; }
    }

    public bool CanSpawnAsteroids
    {
        get { return _canSpawnAsteroids; }
        set { _canSpawnAsteroids = value; }
    }

    public float ChanceToSpawnBulletEnemy
    {
        get { return _chanceToSpawnBulletEnemy; }
        set { _chanceToSpawnBulletEnemy = value; }
    }

    public float ChanceToSpawnLaserEnemy
    {
        get { return _chanceToSpawnLaserEnemy; }
        set { _chanceToSpawnLaserEnemy = value; }
    }

    public float ChanceToSpawnAsteroid
    {
        get { return _chanceToSpawnAsteroid; }
        set { _chanceToSpawnAsteroid = value; }
    }

    public float ChanceToSpawnToken
    {
        get { return _chanceToSpawnToken; }
        set { _chanceToSpawnToken = value; }
    }

    public bool IsNothing
    {
        get { return _isNothing; }
        set { _isNothing = value; }
    }

    #endregion

    void Awake()
    {
        _GM = GetComponent<GameManager>();   
    }

	// Use this for initialization
	void Start () {
        _difficultySetting = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if(!_GM.IsStartingGame)
        {
            _deltaTimeIncrement += Time.deltaTime;
            _deltaDifficultyIncrement += Time.deltaTime;
            _deltaToggleAllIncrement -= Time.deltaTime;

            if(_deltaTimeIncrement > _timeBetweenSpeedIncrements 
                && _GM.GameSpeed >= _maxSpeed)
            {
                _deltaTimeIncrement = 0;
                EventManager.TriggerEvent(EventStrings.SPEED_INCREASE);
            }

            if(_deltaDifficultyIncrement > _timeBetweenDifficultyIncrements
                && _difficultySetting <= 10
                )
            {
                _deltaDifficultyIncrement = 0;
                _difficultySetting++;
                _maxNumberOfAsteroids++;

                print("Difficulty: " + _difficultySetting + " Asteroids: " + _maxNumberOfAsteroids);

            }


            if(Time.time > _timeBeforeAsteroidsAppear)
            {
                _canSpawnAsteroids = true;
            }

            if(_difficultySetting > 2)
            {
                //rint("can spawn laser enemies");
                _canSpawnLaserEnemies = true;
            }

            if(_difficultySetting > 4)
            {
                //print("can spawn bullet enemies");
                _canSpawnBulletEnemies = true;
            }

            if(Time.time > _timeBeforeTokensCanAppear)
            {
                _canSpawnTokens = true;
            }

            if(_deltaToggleAllIncrement < 0)
            {
                
                ToggleAllItems();

                if(_isNothing)
                {
                    _deltaToggleAllIncrement = _timeBetweenToggleIncrements / 4;
                }
                else
                {
                    _deltaToggleAllIncrement = _timeBetweenToggleIncrements;
                }

                print("Toggle: " + _isNothing + " time :" + _deltaToggleAllIncrement);
            }
        }
	}    


    private void ToggleAllItems()
    {
        _canSpawnAsteroids = !_canSpawnAsteroids;
        _canSpawnLaserEnemies = !_canSpawnLaserEnemies;
        _canSpawnBulletEnemies = !_canSpawnBulletEnemies;
        _canSpawnTokens = !_canSpawnTokens;

        _isNothing = !_isNothing;
    }
}
