using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    
    [SerializeField]
    private bool _isDebugInvulne;
    [SerializeField]
    private float _timeUntilPermaDeath;
    [SerializeField]
    private float _gameSpeed;
    [SerializeField]
    private float _playerShieldedTime;
    [SerializeField]
    private float _SpeedTimer;
    [SerializeField]
    private int _maxNumberOfBolts;

    [SerializeField]
    private Vector3 _spawnValues;
    [SerializeField]
    private GameObject _bolt;
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
    private GameObject _ludicrousToken;
    [SerializeField]
    private GameObject _boltToken;
    [SerializeField]
    private GameObject _tweenText;
    [SerializeField]
    private Text _countdownText;
    [SerializeField]
    private GameObject _gameUI;

    private ObjectPoolManager _objPool;
    private List<string> _aliveTokenList;
    private int _destroyedHazards;
    private int _bulletEnemiesAlive;
    private int _laserEnemiesAlive;
    private int _asteroidsAlive;
    private bool _isPlayerShielded;
    private float _deltaShieldTime;
    private float _deltaSpeedTime;
    private bool _isSpeedActive;
    private bool _hasSpeedToken;
    private bool _hasDestroyAllToken;
    private bool _hasShieldToken;
    private bool _speedTokenExists;
    private bool _destroyAllTokenExists;
    private bool _shieldTokenExists;
    private bool _isSpawningAsteroids;
    private bool _hasSpawnStarted;
    private bool _isPlayerDead;
    private int _playerDeathCount;
    private int _currentNumberOfBolts;
    private bool _isStartingGame = true;
    private bool _isWaitingForNewGame;
    private bool _didMurderPlayer;


    #region Properties

    public float PlayerShieldTime
    {
        get { return _playerShieldedTime; }
    }

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

    public bool IsPlayerDead
    {
        get { return _isPlayerDead; }
        set { _isPlayerDead = value; }
    }

    public int PlayerDeathCount
    {
        get { return _playerDeathCount; }
    }

    public float TimeUntilPermaDeath
    {
        get { return _timeUntilPermaDeath; }
    }

    public int MaxNumberOfBullets
    {
        get { return _maxNumberOfBolts; }
        set { _maxNumberOfBolts = value; }
    }

    public int CurrentNumberOfBolts
    {
        get { return _currentNumberOfBolts; }
        set { _currentNumberOfBolts = value; }
    }

    public bool IsWaitingForNewGame {
        get { return _isWaitingForNewGame; }
        set { _isWaitingForNewGame = value; }
    }
        
    #endregion

    void Awake()
    {
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
    }

	// Use this for initialization
	void Start () {
        _objPool.CreateNewDictionary();
        _objPool.CreatePool(20, _coin, _coin.tag);
        _objPool.CreatePool(20, _asteroid, _asteroid.tag);
        _objPool.CreatePool(1, _invulnePower, _invulnePower.tag);
        _objPool.CreatePool(5, _laserEnemy, _laserEnemy.tag);
        _objPool.CreatePool(5, _bulletEnemy, _bulletEnemy.tag);
        _objPool.CreatePool(100, _bullet, _bullet.tag);
        _objPool.CreatePool(1, _destroyAll, _destroyAll.tag);
        _objPool.CreatePool(1, _ludicrousToken, _ludicrousToken.tag);
        _objPool.CreatePool(1, _boltToken, _boltToken.tag);
        _objPool.CreatePool(3, _tweenText, ObjectStrings.TWEEN_TEXT_OUT);
        _objPool.CreatePool(_maxNumberOfBolts, _bolt, _bolt.tag);

        _currentNumberOfBolts = _maxNumberOfBolts;

        _deltaShieldTime = _playerShieldedTime;
        _deltaSpeedTime = _SpeedTimer;

        _aliveTokenList = new List<string>();

        EventManager.TriggerEvent(EventStrings.GET_GAME_MANAGER); 


	}
    #region Enable /Disable Events
    void OnEnable()
    {
        EventManager.StartListening(GameSettings.START_GAME, InstantiatePlayer);
        EventManager.StartListening(GameSettings.RESET_GAME, InstantiatePlayer);
        EventManager.StartListening(GameSettings.MURDER_PLAYER, MurderPlayer);
        EventManager.StartListening(EventStrings.PLAYER_DEAD, PlayerDied);
        EventManager.StartListening(EventStrings.HAZARD_KILL, UpdateHazardDestroyed); 


        EventManager.StartListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StartListening(EventStrings.SPEED_INCREASE, IncreaseGameSpeed);
        EventManager.StartListening(EventStrings.SPEED_DECREASE, DecreaseGameSpeed);
        EventManager.StartListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StartListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StartListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StartListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StartListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);
        EventManager.StartListening(EventStrings.HAZARD_OUT_OF_BOUNDS, DecreaseAliveHazardCount);
        EventManager.StartListening(EventStrings.GRAB_COIN, UpdateCoinScore);

        EventManager.StartListening(EventStrings.GRAB_BOLT_TOKEN, AddBoltToPlayer);
        EventManager.StartListening(EventStrings.PLAYER_SHOOTS, SubtractBoltFromPlayer);


        EventManager.StartListening(GameSettings.GAME_HAS_STARTED, GameStarted);
        EventManager.StartListening(GameSettings.GAME_OVER, ResetGame);

        EventManager.StartListeningForStringEvent(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StartListeningForStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, UpdateAliveList);
        EventManager.StartListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
        EventManager.StartListeningForStringEvent(EventStrings.ENEMY_OUT_OF_BOUNDS, DecreaseAliveEnemyCount);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.START_GAME, InstantiatePlayer);
        EventManager.StopListening(GameSettings.RESET_GAME, InstantiatePlayer);
        EventManager.StopListening(GameSettings.MURDER_PLAYER, MurderPlayer);
        EventManager.StopListening(EventStrings.PLAYER_DEAD, PlayerDied);

        EventManager.StopListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StopListening(EventStrings.SPEED_INCREASE, IncreaseGameSpeed);
        EventManager.StopListening(EventStrings.SPEED_DECREASE, DecreaseGameSpeed);
        EventManager.StopListening(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN, UpdateSpeedTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_DESTROY_ALL_TOKEN, UpdateDestroyAllTokenAvailability);
        EventManager.StopListening(EventStrings.GRAB_INVUNERABILITY_TOKEN, UpdateInvulnerabilityAvailabilityToken);
        EventManager.StopListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StopListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicousSpeed);
        EventManager.StopListening(EventStrings.GET_REKT, UpdateDestroyAllTokenAvailability);
        EventManager.StopListening(EventStrings.HAZARD_OUT_OF_BOUNDS, DecreaseAliveHazardCount);
        EventManager.StopListening(EventStrings.GRAB_COIN, UpdateCoinScore);

        EventManager.StopListening(EventStrings.PLAYER_SHOOTS, SubtractBoltFromPlayer);
        EventManager.StopListening(EventStrings.GRAB_BOLT_TOKEN, AddBoltToPlayer);

        EventManager.StopListening(GameSettings.GAME_HAS_STARTED, GameStarted);
        EventManager.StopListening(GameSettings.GAME_OVER, ResetGame);

        EventManager.StopListeningForStringEvent(EventStrings.ENEMY_DESTROYED, EnemyDestroyed);
        EventManager.StopListeningForStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, UpdateAliveList);
        EventManager.StopListeningForStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ResetToken);
        EventManager.StopListeningForStringEvent(EventStrings.ENEMY_OUT_OF_BOUNDS, DecreaseAliveEnemyCount);
    }
    #endregion

    void Update()
    {


        if(_isPlayerShielded && !_isSpeedActive)
        {
            _deltaShieldTime -= Time.deltaTime;
            if(_deltaShieldTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_OFF);
            }
            else
            {
                _countdownText.enabled = true;
                _countdownText.text = "Shield Time:" + Mathf.RoundToInt(_deltaShieldTime).ToString();
            }
        }

        //if speed is active, so is shield.
        if(_isSpeedActive)
        {
            _deltaSpeedTime -= Time.deltaTime;
            if(_deltaSpeedTime < 0)
            {
                
                EventManager.TriggerEvent(EventStrings.DISENGAGE_LUDICROUS_SPEED);
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_OFF);
            } 
            else
            {
                if(!IsStartingGame)
                {
                    _countdownText.enabled = true;
                    _countdownText.text = "Speed Time:" + Mathf.RoundToInt(_deltaSpeedTime).ToString();    
                }

            }
        }
        if(!IsStartingGame && !IsWaitingForNewGame)
        {
            CalculatePlayDistance();
        }
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
            RemoveTokenFromAliveList(ObjectStrings.LUDICROUS_SPEED);
        }
    }

    public void UpdateDestroyAllTokenExistence()
    {
        _destroyAllTokenExists = !_destroyAllTokenExists;
        if(!_destroyAllTokenExists)
        {
            RemoveTokenFromAliveList(ObjectStrings.DESTROY_ALL);
        }
    }

    public void UpdateInvulnerabilityTokenExistance()
    {
        _shieldTokenExists = !_shieldTokenExists;

        if(!_shieldTokenExists)
        {
            RemoveTokenFromAliveList(ObjectStrings.INVULNERABLE);
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
        _destroyedHazards++;
    }

    private void UpdateCoinScore()
    {
        PlayerData.instance.Scores.lastCoinScore++;
        PlayerData.instance.Scores.globalCoinScore++;

    }

    private void EnemyDestroyed(string tag)
    {
        DecreaseAliveEnemyCount(tag);
        _destroyedHazards++;

    }

    private void AddBoltToPlayer()
    {
        if (_currentNumberOfBolts + 1 <= _maxNumberOfBolts)
        {
            _currentNumberOfBolts++;
        }
    }

    private void SubtractBoltFromPlayer()
    {
        _currentNumberOfBolts--;
    }

    private void CalculatePlayDistance()
    {
        PlayerData.instance.Scores.lastTravelScore += Mathf.Abs(_gameSpeed * Time.deltaTime);
    }
    #endregion

    #region Engage/Disengage Effects
    private void EngageLudicrousSpeed()
    {
        if(!IsStartingGame)
        {
            UpdateSpeedTokenAvailability();    
        }

        //StartCoroutine(Vibrate(_SpeedTimer));
        TogglePlayerAtLudicrousSpeed();
        _gameSpeed -= 30;

    }

    private void DisengageLudicousSpeed()
    {
        TogglePlayerAtLudicrousSpeed();
        _deltaSpeedTime = _SpeedTimer;
        _gameSpeed += 30;
    }

    private void InvulnerabilityOn()
    {

        UpdateInvulnerabilityAvailabilityToken();
        TogglePlayerIsInvulnerable();

    }

    private void InvulnerabilityOff()
    {
        _countdownText.enabled = false;
        _deltaShieldTime = _playerShieldedTime;
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
                DecreaseAliveEnemyCount(name);
                break;
            case "LaserEnemy":
                DecreaseAliveEnemyCount(name);
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

    private void IncreaseGameSpeed()
    {
        _gameSpeed -= 0.1f;
    }

    private void DecreaseGameSpeed()
    {
        if(_gameSpeed <= -5.5)
        {
            _gameSpeed += 0.5f;    
        }

    }

    private void MurderPlayer()
    {
        print("You exited the game");
        _didMurderPlayer = true;
        EventManager.TriggerEvent(GameSettings.GAME_OVER);

    }

    private void PlayerDied()
    {
        print("You died");
        _isPlayerDead = true;
        _playerDeathCount++;
        StartCoroutine(WaitForDeath("Natural"));
    }
    #endregion


    private IEnumerator WaitForDeath(string causeOfDeath)
    {
        yield return new WaitForSeconds(0.5f);

        if(!_didMurderPlayer)
        {
            EventManager.TriggerEvent(MenuStrings.ENABLE_GAMEOVER_MENU);    
        }
        else
        {
            //Nothing. its cool.
        }
            
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
            case ObjectStrings.INVULNERABLE:
                UpdateInvulnerabilityTokenExistance();
                break;
            case ObjectStrings.DESTROY_ALL:
                UpdateDestroyAllTokenExistence();
                break;
            case ObjectStrings.LUDICROUS_SPEED:
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

    private void InstantiatePlayer()
    {
        EventManager.TriggerEvent(MenuStrings.ENABLE_INTRO_SCREEN);
        PlayerData.instance.Scores.lastCoinScore = 0;
        PlayerData.instance.Scores.lastTravelScore = 0;
        _isPlayerDead = false;
        _isStartingGame = true;
        GetComponent<CreatePlayer>().Create();
    }

    private void GameStarted()
    {
        _gameUI.SetActive(true);
        _isStartingGame = false;
        _isWaitingForNewGame = false;
        EventManager.TriggerEvent(MenuStrings.DISABLE_INTRO_SCREEN);
    }

    private void ResetGame()
    {
        _deltaShieldTime = _playerShieldedTime;
        _deltaSpeedTime = _SpeedTimer;
        _aliveTokenList = new List<string>();

        _hasSpeedToken = false;
        _hasShieldToken = false;
        _hasDestroyAllToken = false;
        _isStartingGame = false;

        _playerDeathCount = 0;
        _currentNumberOfBolts = _maxNumberOfBolts;

        _didMurderPlayer = false;

        _isWaitingForNewGame = true;
    }
}
   
