﻿using UnityEngine;
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
    private float _timeBeforeTokensCanAppear;
    [SerializeField]
    private int _maxNumberOfLaserEnemies;
    [SerializeField]
    private int _maxNumberOfBulletEnemies;
    [SerializeField]
    private int _maxNumberOfAsteroids;
    [SerializeField]
    private int _maxNumberOfEnemiesOnScreen;
    [SerializeField]
    private float _chanceToSpawnToken;
    [SerializeField]
    private float _chanceToSpawnBulletEnemy;
    [SerializeField]
    private float _chanceToSpawnLaserEnemy;
    [SerializeField]
    private float _chanceToSpawnAsteroid;

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

    public int MaxNumberOfEnemiesOnScreen
    {
        get { return _maxNumberOfEnemiesOnScreen; }
        set { _maxNumberOfEnemiesOnScreen = value; }
    }

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

                //print("Difficulty: " + _difficultySetting + " Asteroids: " + _maxNumberOfAsteroids);

            }


            if(Time.time > _timeBeforeAsteroidsAppear)
            {
                _canSpawnAsteroids = true;
            }

            if(_difficultySetting == 1)
            {
                print("Level: " + _difficultySetting);
            }

            if(_difficultySetting == 2)
            {
                print("Level: " + _difficultySetting);

                _canSpawnLaserEnemies = true;
            }

            if(_difficultySetting == 3)
            {
                print("Level: " + _difficultySetting);
                _maxNumberOfLaserEnemies = 2;
            }

            if(_difficultySetting == 4)
            {
                print("Level: " + _difficultySetting);
                _maxNumberOfLaserEnemies = 3;
                _canSpawnBulletEnemies = true;
            }

            if(_difficultySetting == 5)
            {
                print("Level: " + _difficultySetting);
                _canSpawnLaserEnemies = false;

            }

            if(_difficultySetting == 6)
            {
                print("Level: " + _difficultySetting);
                _maxNumberOfBulletEnemies = 3;
            }

            if(_difficultySetting == 7)
            {
                print("Level: " + _difficultySetting);
            }

            if(_difficultySetting == 8)
            {
                print("Level: " + _difficultySetting);
                _canSpawnLaserEnemies = true;
                _maxNumberOfLaserEnemies = 2;
                _maxNumberOfBulletEnemies = 2;
            }

            if(_difficultySetting == 9)
            {
                print("Level: " + _difficultySetting);
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

                //print("Toggle: " + _isNothing + " time :" + _deltaToggleAllIncrement);
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
