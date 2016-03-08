using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    [SerializeField]
    private bool _isDebugInvulne = true;
    [SerializeField]
    private float _gameSpeed = -5.0f;
    [SerializeField]
    private float _playerShieldedTimer = 3.0f;
    [SerializeField]
    private float _SpeedTimer = 3.0f;
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
    private GameObject _bullet;
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
    private List<string> _aliveTokenList;
    private float _playDistance = 0;
    private int _destroyedHazards = 0;
    private int _bulletEnemiesAlive;
    private int _laserEnemiesAlive;
    private int _asteroidsAlive;
    private bool _isPlayerShielded;
    private float _deltaShieldTime;
    private float _deltaSpeedTime;
    private int _coinScore;
    private bool _isSpeedActive;
    private bool _hasSpeedToken;
    private bool _hasDestroyAllToken;
    private bool _hasShieldToken;
    private bool _speedTokenExists;
    private bool _destroyAllTokenExists;
    private bool _shieldTokenExists;
    private bool _isSpawningAsteroids;
    private bool _hasSpawnStarted;
    private bool _hasDestroyAllTokenHelpBeenDisplayed;
    private bool _hasShieldTokenHelpBeenDisplayed;
    private bool _hasSpeedTokenHelpBeenDisplayed;
    private bool _isStartingGame = true;

    #region Properties

    public List<string> AliveTokenList
    {
        get { return _aliveTokenList; }
        set { _aliveTokenList = value; }
    }

    public bool DebugInvulne
    {
        get { return _isDebugInvulne; }
        set { _isDebugInvulne = value; }
    }

    public int CoinScore
    {
        get { return _coinScore; }
        set { _coinScore = value; }
    }
        
    public float GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }
       
    public bool IsPlayerShielded
    {
        get { return _isPlayerShielded; }
        set { _isPlayerShielded = value; }
    }

    public bool IsSpeedActive
    {
        get { return _isSpeedActive; }
        set { _isSpeedActive = value; }
    }

    public bool HasSpeedToken
    {
        get { return _hasSpeedToken; }
        set { _hasSpeedToken = value; }
    }

    public bool HasDestroyAllToken
    {
        get { return _hasDestroyAllToken; }
        set { _hasDestroyAllToken = value; }
    }

    public bool HasShieldToken
    {
        get { return _hasShieldToken; }
        set { _hasShieldToken = value; }
    }

    public bool SpeedTokenExists
    {
        get { return _speedTokenExists; }
        set { _speedTokenExists = value; }
    }

    public bool DestroyAllTokenExists
    {
        get { return _destroyAllTokenExists; }
        set { _destroyAllTokenExists = value; }
    }

    public bool ShieldTokenExists
    {
        get { return _shieldTokenExists; }
        set { _shieldTokenExists = value; }
    }

    public bool HasDestroyTokenHelpBeenDisplayed
    {
        get { return _hasDestroyAllTokenHelpBeenDisplayed; }
        set { _hasDestroyAllTokenHelpBeenDisplayed = value; }
    }

    public bool HasShieldTokenHelpBeenDisplayed
    {
        get { return _hasShieldTokenHelpBeenDisplayed; }
        set { _hasShieldTokenHelpBeenDisplayed = value; }
    }

    public bool HasSpeedTokenHelpBeenDisplayed
    {
        get { return _hasSpeedTokenHelpBeenDisplayed; }
        set { _hasSpeedTokenHelpBeenDisplayed = value; }
    }

    public int LaserEnemiesAlive
    {
        get { return _laserEnemiesAlive; }
        set { _laserEnemiesAlive = value; }
    }

    public int BulletEnemiesAlive
    {
        get { return _bulletEnemiesAlive; }
        set { _bulletEnemiesAlive = value; }
    }

    public int AsteroidsAlive
    {
        get { return _asteroidsAlive; }
        set { _asteroidsAlive = value; }
    }

    public bool IsStartingGame
    {
        get { return _isStartingGame; }
        set { _isStartingGame = value; }
    }
        
    #endregion

    void Awake()
    {
//        if (instance == null)
//        {
//            instance = this;
//        } else
//        {
//            if(this != instance)
//                Destroy(this.gameObject);
//        }
//
//        DontDestroyOnLoad(this);
//
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
        _objPool.CreatePool(300, _bullet, _bullet.tag);
        _objPool.CreatePool(10, _destroyAll, _destroyAll.tag);
        _objPool.CreatePool(10, _ludicrousToken, _ludicrousToken.tag);

        _killText.text = "Kills: 0";
        _distanceText.text = "Distance: 0";
        _speedText.text = "Speed: 5";
        _scoreText.text = "Score: 0";

        _deltaShieldTime = _playerShieldedTimer;
        _deltaSpeedTime = _SpeedTimer;

        _aliveTokenList = new List<string>();

        CheckPlayerPrefs();



	}
    #region Enable /Disable Events
    void OnEnable()
    {
        EventManager.StartListening(EventStrings.PLAYER_DEAD, GameOver);
        EventManager.StartListening(EventStrings.HAZARD_KILL, UpdateHazardDestroyed); 
        EventManager.StartListening(EventStrings.GRAB_COIN, UpdateCoinScore);
        EventManager.StartListening(EventStrings.GRAB_BIG_COIN, UpdateBigCoinScore);

        EventManager.StartListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StartListening(EventStrings.SPEED_INCREASE, UpdateGameSpeed);
        EventManager.StartListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StartListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StartListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StartListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);
        EventManager.StartListening(EventStrings.HAZARD_OUT_OF_BOUNDS, DecreaseAliveHazardCount);


        EventManager.StartListening(GameSettings.GAME_STARTED, GameStarted);

        EventManager.StartListeningForStringEvent(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StartListeningForStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, UpdateAliveList);
        EventManager.StartListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
        EventManager.StartListeningForStringEvent(EventStrings.ENEMY_OUT_OF_BOUNDS, DecreaseAliveEnemyCount);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.PLAYER_DEAD, GameOver);
        EventManager.StopListening(EventStrings.HAZARD_KILL, UpdateHazardDestroyed);
        EventManager.StopListening(EventStrings.GRAB_COIN, UpdateCoinScore);
        EventManager.StopListening(EventStrings.GRAB_BIG_COIN, UpdateBigCoinScore);

        EventManager.StopListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StopListening(EventStrings.SPEED_INCREASE, UpdateGameSpeed);
        EventManager.StopListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StopListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StopListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StopListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);
        EventManager.StopListening(EventStrings.HAZARD_OUT_OF_BOUNDS, DecreaseAliveHazardCount);

        EventManager.StopListening(GameSettings.GAME_STARTED, GameStarted);

        EventManager.StopListeningForStringEvent(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StopListeningForStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, UpdateAliveList);
        EventManager.StopListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
        EventManager.StopListeningForStringEvent(EventStrings.ENEMY_OUT_OF_BOUNDS, DecreaseAliveEnemyCount);
    }
    #endregion

    void Update()
    {
        _timePlayedText.text = Mathf.RoundToInt(Time.time).ToString();
            

        if(_isPlayerShielded)
        {
            _deltaShieldTime -= Time.deltaTime;
            if(_deltaShieldTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_OFF);
            }
        }

        if(_isSpeedActive)
        {
            _deltaSpeedTime -= Time.deltaTime;
            if(_deltaSpeedTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.STOP_CAMERA_SHAKE);
                EventManager.TriggerEvent(EventStrings.DISENGAGE_LUDICROUS_SPEED);
            }
        }

        CalculatePlayDistance();
    }


    #region Token Availability
    public void UpdateSpeedTokenAvailability()
    {
        _hasSpeedToken = !_hasSpeedToken;
    }

    public void UpdateDestroyAllTokenAvailability()
    {
        _hasDestroyAllToken = !_hasDestroyAllToken;
    }

    public void UpdateInvulnerabilityAvailabilityToken()
    {
        _hasShieldToken = !_hasShieldToken;
    }
    #endregion

    #region Token existance
    public void UpdateSpeedTokenExistence()
    {
        _speedTokenExists = !_speedTokenExists;
        if(!_speedTokenExists)
        {
            RemoveTokenFromAliveList(TagStrings.LUDICROUS_SPEED);
        }
    }

    public void UpdateDestroyAllTokenExistence()
    {
        _destroyAllTokenExists = !_destroyAllTokenExists;
        if(!_destroyAllTokenExists)
        {
            RemoveTokenFromAliveList(TagStrings.DESTROY_ALL);
        }
    }

    public void UpdateInvulnerabilityTokenExistance()
    {
        _shieldTokenExists = !_shieldTokenExists;

        if(!_shieldTokenExists)
        {
            RemoveTokenFromAliveList(TagStrings.INVULNERABLE);
        }
    }
    #endregion


    #region toggle player effects
    private void TogglePlayerAtLudicrousSpeed()
    {
        _isSpeedActive = !_isSpeedActive;
    }

    private void TogglePlayerIsInvulnerable()
    {
        _isPlayerShielded = !_isPlayerShielded;
    }
    #endregion


    #region Update Scores and stats

    private void DecreaseAliveHazardCount()
    {
        _asteroidsAlive--;
    }

    private void DecreaseAliveEnemyCount(string enemyType)
    {
        switch (enemyType)
        {
            case "BulletEnemy":
                _bulletEnemiesAlive--;
                break;
            case "LaserEnemy":
                _laserEnemiesAlive--;
                break;
            default:
                break;
        }
    }
    private void UpdateHazardDestroyed()
    {
        DecreaseAliveHazardCount();
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

    private void EnemyDestroyed(string tag)
    {
        DecreaseAliveEnemyCount(tag);

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
        _deltaSpeedTime = _SpeedTimer;
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
        _deltaShieldTime = _playerShieldedTimer;
        TogglePlayerIsInvulnerable();
        UpdateInvulnerabilityTokenExistance();
    }
    #endregion

    #region Maintain Game
    private void UpdateAliveList(string name)
    {
        switch (name)
        {
            case "Hazard":
                DecreaseAliveHazardCount();
                break;
            case "BulletEnemy":
                EnemyDestroyed(name);
                break;
            case "LaserEnemy":
                EnemyDestroyed(name);
                break;
            case "Invulnerable":    
                RemoveTokenFromAliveList(name);
                break;
            case "DestroyAll":
                RemoveTokenFromAliveList(name);
                break;
                case "LudicrousSpeed":
                RemoveTokenFromAliveList(name);
                break;
            default:
                break;
        }
    }

    private void UpdateGameSpeed()
    {
        _gameSpeed -= 0.1f;
        _speedText.text = Mathf.Abs(_gameSpeed).ToString("F1");
    }

    private void GameOver()
    {
        print("You died");
        StartCoroutine(WaitForDeath());
    }
    #endregion


    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(0.5f);
        EventManager.TriggerEvent(EventStrings.ENABLE_GAMEOVER_MENU);


    }

    private void MaintainPositionLists(List<Vector3> list)
    {
        //print("List count : " +list.Count);

        if(list.Count < 10)
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

    private void RemoveTokenFromAliveList(string name)
    {
        for (int i = 0; i < _aliveTokenList.Count; i++)
        {
            if(name == _aliveTokenList[i])
            {
                _aliveTokenList.Remove(_aliveTokenList[i]);
                break;
            }
        }
    }

    private void GameStarted()
    {
        _isStartingGame = false;
    }

    private void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey(GameSettings.HAS_DESTROY_ALL_TOKEN_HELP_BEEN_DISPLAYED))
        {
            _hasDestroyAllTokenHelpBeenDisplayed = true;
        }

        if (PlayerPrefs.HasKey(GameSettings.HAS_SHIELD_TOKEN_HELP_BEEN_DISPLAYED))
        {
            _hasShieldTokenHelpBeenDisplayed = true;
        }

        if (PlayerPrefs.HasKey(GameSettings.HAS_SPEED_TOKEN_HELP_BEEN_DISPLAYED))
        {
            _hasSpeedTokenHelpBeenDisplayed = true;
        }
    }

}
   
