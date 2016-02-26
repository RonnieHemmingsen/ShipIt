using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    [SerializeField]
    private bool _isDebugInvulne = true;
    [SerializeField]
    private float _gameSpeed = -5.0f;
    [SerializeField]
    private float _spawnWait = 1.5f;
    [SerializeField]
    private float _startWait = 1f;
    [SerializeField]
    private float _waveWait = 3f;
    [SerializeField]
    private int _hazardCount = 10;
    [SerializeField]
    private float _playerInvulnerableTimer = 3.0f;
    [SerializeField]
    private float _ludicrousSpeedTimer = 3.0f;
    [SerializeField]
    private float _chanceToSpawnDestroyAll = 0.01f;
    [SerializeField]
    private float _chanceToSpawnInvulnerability = 0.03f;
    [SerializeField]
    private float _chanceToSpawnLaserEnemy = 0.08f;
    [SerializeField]
    private float _chanceToSpawnBulletEnemy = 0.2f;
    [SerializeField]
    private float _chanceToSpawnCoin = 1f;
    [SerializeField]
    private float _chanceToSpawnBigCoin = 0.02f;
    [SerializeField]
    private float _chanceToSpawnLudicrousSpeed = 0.2f;
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
    private Vector3 _spawnValues;
    [SerializeField]
    private GameObject _asteroid;
    [SerializeField]
    private GameObject _laserEnemy;
    [SerializeField]
    private GameObject _bulletEnemy;
    [SerializeField]
    private GameObject _invulnePower;
    [SerializeField]
    private GameObject _destroyAll;
    [SerializeField]
    private GameObject _coin;
    [SerializeField]
    private GameObject _bigCoin;
    [SerializeField]
    private GameObject _ludicrousToken;

    private GameBalancer _zen;
    private ObjectPoolManager _objPool;
    private List<Vector3> ItemSpawnPositions = new List<Vector3>();
    private List<Vector3> EnemySpawnPositions = new List<Vector3>();
    private float _playDistance = 0;
    private int _destroyedHazards = 0;
    private bool _isPlayerInvulnerable;
    private float _deltaInvulneTime;
    private float _deltaLudicrousSpeedTime;
    private bool _isDestroyAllSpawned;
    private int _coinScore;
    private bool _canSpawnLaserEnemies;
    private bool _canSpawnBulletEnemies;
    private bool _canSpawnAsteroids;
    private bool _isLudicrousSpeedActive;
    private bool _hasLudicrousSpeedToken;
    private bool _hasDestroyAllToken;
    private bool _hasInvulnerabilityToken;
    private bool _ludicrousTokenExists;
    private bool _destroyAllTokenExists;
    private bool _invulnerabilityTokenExists;

    #region Properties
    public bool DebugInvulne
    {
        get { return _isDebugInvulne; }
        set { _isDebugInvulne = value; }
    }
        
    public float GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }
       
    public bool IsPlayerInvulnerable
    {
        get { return _isPlayerInvulnerable; }
        set { _isPlayerInvulnerable = value; }
    }

    public bool IsLudicrousSpeedActive
    {
        get { return _isLudicrousSpeedActive; }
        set { _isLudicrousSpeedActive = value; }
    }

    public bool HasLudicrousSpeedToken
    {
        get { return _hasLudicrousSpeedToken; }
        set { _hasLudicrousSpeedToken = value; }
    }

    public bool HasDestroyAllToken
    {
        get { return _hasDestroyAllToken; }
        set { _hasDestroyAllToken = value; }
    }

    public bool HasInvulnerabilityToken
    {
        get { return _hasInvulnerabilityToken; }
        set { _hasInvulnerabilityToken = value; }
    }

    public bool LudicrousTokenExists
    {
        get { return _ludicrousTokenExists; }
        set { _ludicrousTokenExists = value; }
    }

    public bool DestroyAllTokenExists
    {
        get { return _destroyAllTokenExists; }
        set { _destroyAllTokenExists = value; }
    }

    public bool InvulnerabilityTokenExists
    {
        get { return _invulnerabilityTokenExists; }
        set { _invulnerabilityTokenExists = value; }
    }


    public bool CanSpawnAsteroids
    {
        get { return _canSpawnAsteroids; }
        set { _canSpawnAsteroids = value; }
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
        
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if(this != instance)
                Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
        _zen = GetComponent<GameBalancer>();
    }



	// Use this for initialization
	void Start () {
        _objPool.CreatePool(100, _coin, _coin.tag);
        _objPool.CreatePool(100, _bigCoin, _bigCoin.tag);
        _objPool.CreatePool(20, _asteroid, _asteroid.tag);
        _objPool.CreatePool(10, _invulnePower, _invulnePower.tag);
        _objPool.CreatePool(_zen.MaxNumberOfLaserEnemies, _laserEnemy, _laserEnemy.tag);
        _objPool.CreatePool(_zen.MaxNumberOfLaserEnemies, _bulletEnemy, _bulletEnemy.tag);
        _objPool.CreatePool(10, _destroyAll, _destroyAll.tag);
        _objPool.CreatePool(10, _ludicrousToken, _ludicrousToken.tag);

        _killText.text = "Kills: 0";
        _distanceText.text = "Distance: 0";
        _speedText.text = "Speed: 5";
        _scoreText.text = "Score: 0";

        _deltaInvulneTime = _playerInvulnerableTimer;
        _deltaLudicrousSpeedTime = _ludicrousSpeedTimer;

        InvokeRepeating("SpawnDecider", 1.0f, .5f);
        StartCoroutine(SpawnAsteroidWaves());

	}

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.PLAYER_DEAD, GameOver);
        EventManager.StartListening(EventStrings.HAZARD_KILL, UpdateHazardDestroyed); 
        EventManager.StartListening(EventStrings.GRAB_COIN, UpdateCoinScore);
        EventManager.StartListening(EventStrings.GRAB_BIG_COIN, UpdateBigCoinScore);
        EventManager.StartListening(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StartListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StartListening(EventStrings.SPEED_INCREASE, UpdateGameSpeed);
        EventManager.StartListening(EventStrings.TOGGLE_LASER_ENEMY_SPAWNING, ToggleLaserEnemySpawn);
        EventManager.StartListening(EventStrings.TOGGLE_ASTEROID_SPAWNING, ToggleAsteroidSpawn);
        EventManager.StartListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StartListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StartListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StartListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);

        EventManager.StartListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.PLAYER_DEAD, GameOver);
        EventManager.StopListening(EventStrings.HAZARD_KILL, UpdateHazardDestroyed);
        EventManager.StopListening(EventStrings.GRAB_COIN, UpdateCoinScore);
        EventManager.StopListening(EventStrings.GRAB_BIG_COIN, UpdateBigCoinScore);
        EventManager.StopListening(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StopListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StopListening(EventStrings.SPEED_INCREASE, UpdateGameSpeed);
        EventManager.StopListening(EventStrings.TOGGLE_LASER_ENEMY_SPAWNING, ToggleLaserEnemySpawn);
        EventManager.StopListening(EventStrings.TOGGLE_ASTEROID_SPAWNING, ToggleAsteroidSpawn);
        EventManager.StopListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StopListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StopListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StopListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);

        EventManager.StopListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
    }

    void Update()
    {
        _timePlayedText.text = Mathf.RoundToInt(Time.time).ToString();

        if(_isPlayerInvulnerable)
        {
            _deltaInvulneTime -= Time.deltaTime;
            if(_deltaInvulneTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_OFF);
            }
        }

        if(_isLudicrousSpeedActive)
        {
            _deltaLudicrousSpeedTime -= Time.deltaTime;
            if(_deltaLudicrousSpeedTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.STOP_CAMERA_SHAKE);
                EventManager.TriggerEvent(EventStrings.DISENGAGE_LUDICROUS_SPEED);
            }
        }
            
        CalculatePlayDistance();
    }

    #region Spawn methods
    private void SpawnDecider()
    {
        float rand = Random.value;

        if(rand <= _chanceToSpawnCoin) 
        {
            StartCoroutine(SpawnStuff(TagStrings.COIN, 0));    
        }

        if(rand <= _chanceToSpawnBigCoin)
        {
            StartCoroutine(SpawnStuff(TagStrings.BIG_COIN, 0));
        }

        if (rand <= _chanceToSpawnBulletEnemy) 
        {
            float randTime = Random.Range(0.0f, 1.0f);
            if(_canSpawnBulletEnemies)
            {
                StartCoroutine(SpawnStuff(TagStrings.BULLET_ENEMY, randTime));   
            }
        }

        if (rand <= _chanceToSpawnLaserEnemy) 
        {
            float randTime = Random.Range(0.0f, 1.0f);
            if(_canSpawnLaserEnemies)
            {
                StartCoroutine(SpawnStuff(TagStrings.LASER_ENEMY, randTime));   
            }

        }

        if (rand <= _chanceToSpawnDestroyAll && !_destroyAllTokenExists) 
        {
            float randTime = Random.Range(0.0f, 1.0f);
            StartCoroutine(SpawnStuff(TagStrings.DESTROY_ALL, randTime));
            UpdateDestroyAllTokenExistence();
        }

        if (rand <= _chanceToSpawnInvulnerability && !_isPlayerInvulnerable && !_invulnerabilityTokenExists) 
        {
            float randTime = Random.Range(0.0f, 1.0f);
            StartCoroutine(SpawnStuff(TagStrings.INVULNERABLE, randTime));    
            UpdateInvulnerabilityTokenExistance();


        }

        if (rand <= _chanceToSpawnLudicrousSpeed && !_isLudicrousSpeedActive && !_ludicrousTokenExists)
        {
            float randTime = Random.Range(0.0f, 1.0f);
            StartCoroutine(SpawnStuff(TagStrings.LUDICROUS_SPEED, randTime));    
            UpdateSpeedTokenExistence();
        }
    }

    private IEnumerator SpawnAsteroidWaves()
    {
        
        yield return new WaitForSeconds(_startWait);
        while(_canSpawnAsteroids)
        {
            for (int i = 0; i < _hazardCount; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

                GameObject GO = _objPool.GetObjectFromPool(TagStrings.HAZARD);
                if(GO != null)
                {
                    GO.transform.position = spawnPos;    
                }
                yield return new WaitForSeconds(_spawnWait);

            }    
            yield return new WaitForSeconds(_waveWait);
        }
    }

    private IEnumerator SpawnStuff(string tag, float randTime)
    {
        bool canSpawnHere = false;
        Vector3 spawnPos = Vector3.zero;

        yield return new WaitForSeconds(randTime);

        while(!canSpawnHere)
        {
            spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

            if(EnemySpawnPositions.Count == 0)
            {
                EnemySpawnPositions.Add(spawnPos);
                canSpawnHere = true;
            }

            if(tag == TagStrings.LASER_ENEMY || tag == TagStrings.BULLET_ENEMY)
            {
                foreach (var item in EnemySpawnPositions)
                {
                    if(Utilities.DistanceLessThanValueToOther(spawnPos, item, 5.0f))
                    {
                        canSpawnHere = true;
                        break;
                    }
                }
            }
            else
            {
                if(ItemSpawnPositions.Count == 0)
                {
                    ItemSpawnPositions.Add(spawnPos);
                    canSpawnHere = true;
                }
                foreach (var item in ItemSpawnPositions)
                {
                    if(Utilities.DistanceLessThanValueToOther(spawnPos, item, 1.0f))
                    {
                        //print(spawnPos + " - " + item);
                        canSpawnHere = true;
                        break;
                    }
                }
            }
        }

        if(tag == TagStrings.BULLET_ENEMY || tag == TagStrings.LASER_ENEMY)
        {
            EnemySpawnPositions.Add(spawnPos);
            MaintainPositionLists(EnemySpawnPositions);
        }
        else
        {
            ItemSpawnPositions.Add(spawnPos);
            MaintainPositionLists(ItemSpawnPositions);
        }

        GameObject GO = _objPool.GetObjectFromPool(tag);
        if(GO != null)
        {
            GO.transform.position = spawnPos; 
        }
        else
        {
            //debug only
            print("Attempted to spawn null object: " + tag);
        }

    }
    #endregion

    #region toggle player effects
    private void TogglePlayerAtLudicrousSpeed()
    {
        _isLudicrousSpeedActive = !_isLudicrousSpeedActive;
    }

    private void TogglePlayerIsInvulnerable()
    {
        _isPlayerInvulnerable = !_isPlayerInvulnerable;
    }
    #endregion

    #region Token Availability
    private void UpdateSpeedTokenAvailability()
    {
        _hasLudicrousSpeedToken = !_hasLudicrousSpeedToken;
    }

    private void UpdateDestroyAllTokenAvailability()
    {
        _hasDestroyAllToken = !_hasDestroyAllToken;
    }

    private void UpdateInvulnerabilityAvailabilityToken()
    {
        _hasInvulnerabilityToken = !_hasInvulnerabilityToken;
    }
    #endregion

    #region Token existance
    private void UpdateSpeedTokenExistence()
    {
        _ludicrousTokenExists = !_ludicrousTokenExists;
    }

    private void UpdateDestroyAllTokenExistence()
    {
        _destroyAllTokenExists = !_destroyAllTokenExists;
    }

    private void UpdateInvulnerabilityTokenExistance()
    {
        _invulnerabilityTokenExists = !_invulnerabilityTokenExists;
    }
    #endregion

    #region Toggle Obstacle Spawn
    private void ToggleLaserEnemySpawn()
    {
        _canSpawnLaserEnemies = !_canSpawnLaserEnemies;
    }

    private void ToggleAsteroidSpawn()
    {
        _canSpawnAsteroids = !_canSpawnAsteroids;

        if(_canSpawnAsteroids)
        {
            StartCoroutine(SpawnAsteroidWaves());
        }
    }
    #endregion

    #region Update Scores and stats
    private void UpdateHazardDestroyed()
    {
        _destroyedHazards += 1;
        _coinScore += 5;
        _scoreText.text = "Score: " + _coinScore.ToString();

    }

    private void UpdateCoinScore()
    {
        _coinScore += 1;
        _scoreText.text = "Score: " + _coinScore.ToString();
    }

    private void UpdateBigCoinScore()
    {
        _coinScore += 100;
        _scoreText.text = "Score: " +_coinScore.ToString();
    }

    private void EnemyDestroyed()
    {
        _destroyedHazards += 1;
        _coinScore += 10;
        _scoreText.text = "Score: " + _coinScore.ToString(); 

    }

    private void CalculatePlayDistance()
    {
        _playDistance = Mathf.Abs(_gameSpeed * Time.time / 100);
        _distanceText.text = _playDistance.ToString("F1");
    }
    #endregion

    #region Engage/Disengage Effects
    private void EngageLudicrousSpeed()
    {
        UpdateSpeedTokenAvailability();
        TogglePlayerAtLudicrousSpeed();
        TogglePlayerIsInvulnerable();
        _gameSpeed -= 30;
        _speedText.text = _gameSpeed.ToString();
    }

    private void DisengageLudicousSpeed()
    {
        TogglePlayerAtLudicrousSpeed();
        TogglePlayerIsInvulnerable();
        _deltaLudicrousSpeedTime = _ludicrousSpeedTimer;
        _gameSpeed += 30;
        _speedText.text = _gameSpeed.ToString();
    }

    private void InvulnerabilityOn()
    {
        UpdateInvulnerabilityAvailabilityToken();
        TogglePlayerIsInvulnerable();

    }

    private void InvulnerabilityOff()
    {
        _deltaInvulneTime = _playerInvulnerableTimer;
        TogglePlayerIsInvulnerable();
        UpdateInvulnerabilityTokenExistance();
    }
    #endregion

    #region Maintain Game
    private void UpdateGameSpeed()
    {
        _gameSpeed -= 0.1f;
        _speedText.text = Mathf.Abs(_gameSpeed).ToString("F1");
    }

    private void GameOver()
    {
        this.enabled = false;
        Destroy(this.gameObject);

        SceneManager.LoadScene("Game");
        //_lvlMan.LoadLevel("Start Menu");

        print("You died");
    }
    #endregion

    private void MaintainPositionLists(List<Vector3> list)
    {
        //print("List count : " +list.Count);

        if(list.Count < 5)
            return;

        list.RemoveAt(0);
    }

    private void ResetToken(string tag)
    {
        switch (tag)
        {
            case TagStrings.INVULNERABLE:
                UpdateInvulnerabilityTokenExistance();
                break;
            case TagStrings.DESTROY_ALL:
                UpdateDestroyAllTokenExistence();
                break;
            case TagStrings.LUDICROUS_SPEED:
                UpdateSpeedTokenExistence();
                break;

            default:
                break;
        }
    }

}
   
