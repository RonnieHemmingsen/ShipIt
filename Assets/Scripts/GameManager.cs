using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    [SerializeField]
    private bool _isDebugInvulne = true;
    [SerializeField]
    private float _gameSpeed = -5.0f;
    [SerializeField]
    private float _scoreMultiplier = 10.0f;
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
    private float _spawnEnemyTimer = 20.0f;
    [SerializeField]
    private int _chanceToSpawnDestroyAll = 10;
    [SerializeField]
    private int _chanceToSpawnInvulnerability = 10;
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
    private GameObject _enemy;
    [SerializeField]
    private GameObject _invulnePower;
    [SerializeField]
    private GameObject _destroyAll;
    [SerializeField]
    private GameObject _coin;


    private ObjectPoolManager _objPool;
    private float _score = 0;
    private float _playTime = 0;
    private float _playDistance = 0;
    private int _destroyedHazards = 0;
    private bool _isPlayerInvulnerable;
    private bool _isEnemySpawned;
    private bool _enemyCanSpawn;
    private float _deltaInvulneTime;
    private float _deltaEnemyTime;
    private bool _isDestroyAllSpawned;
    private int _coinScore;

    public bool DebugInvulne
    {
        get { return _isDebugInvulne; }
        set { _isDebugInvulne = value; }
    }

    public float Score {
        get { return _score; }
        set { _score = value; }
    }

    public float PlayDistance
    {
        get { return _playDistance; }
        set { _playDistance = value; }
    }

    public float GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }

    public int DestroyedHazards
    {
        get { return _destroyedHazards; }
        set { _destroyedHazards = value; }
    }
       
    public bool IsPlayerInvulnerable
    {
        get { return _isPlayerInvulnerable; }
        set { _isPlayerInvulnerable = value; }
    }

    public int CoinScore
    {
        get { return _coinScore; }
        set { _coinScore = value; }
    }

    public bool IsEnemySpawned
    {
        get { return _isEnemySpawned; }
        set { _isEnemySpawned = value; }
    }
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
    }



	// Use this for initialization
	void Start () {
        _objPool.CreatePool(100, _coin, _coin.tag);
        _objPool.CreatePool(20, _asteroid, _asteroid.tag);
        _objPool.CreatePool(10, _invulnePower, _invulnePower.tag);
        _objPool.CreatePool(5, _enemy, _enemy.tag);
        _objPool.CreatePool(10, _destroyAll, _destroyAll.tag);

        _killText.text = "Kills: 0";
        _distanceText.text = "Distance: 0";
        _speedText.text = "Speed: 5";
        _scoreText.text = "Score: 0";

        _deltaEnemyTime = _spawnEnemyTimer;
        _deltaInvulneTime = _playerInvulnerableTimer;

        _enemyCanSpawn = true;

        StartCoroutine(SpawnWaves());
	}

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.PLAYERDEAD, GameOver);
        EventManager.StartListening(EventStrings.HAZARDKILL, UpdateHazardDestroyed); 
        EventManager.StartListening(EventStrings.COINGRAB, UpdateCoinScore);
        EventManager.StartListening(EventStrings.ENEMYDESTROYED, EnemyDestroyed);
        EventManager.StartListening(EventStrings.INVULNERABILITYON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITYOFF, InvulnerabilityOff);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.PLAYERDEAD, GameOver);
        EventManager.StopListening(EventStrings.HAZARDKILL, UpdateHazardDestroyed);
        EventManager.StopListening(EventStrings.COINGRAB, UpdateCoinScore);
        EventManager.StopListening(EventStrings.ENEMYDESTROYED, EnemyDestroyed);
        EventManager.StopListening(EventStrings.INVULNERABILITYON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITYOFF, InvulnerabilityOff);
    }

    void Update()
    {
        _playTime += Mathf.Round(Time.time * 100) / 100f;
        _timePlayedText.text = _playTime.ToString();

        if(_isPlayerInvulnerable)
        {
            _deltaInvulneTime -= Time.deltaTime;
            if(_deltaInvulneTime < 0)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITYOFF);
                _isPlayerInvulnerable = false;
                _deltaInvulneTime = _playerInvulnerableTimer;
            }
        }
        //print(IsEnemySpawned);
        if(!IsEnemySpawned)
        {
            
            _deltaEnemyTime -= Time.deltaTime;
            if(_deltaEnemyTime < 0)
            {
                _enemyCanSpawn = true;
            }
        }


        CalculateScore();
        CalculatePlayDistance();
        CalculateGameSpeed();
    }

    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(_startWait);
        while(true)
        {
            for (int i = 0; i < _hazardCount; i++)
            {
                Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

                GameObject GO = _objPool.GetObjectFromPool(TagStrings.HAZARD);
                GO.transform.position = spawnPos;

                SpawnCoins(spawnPos);
                SpawnEnemies();
                SpawnDestroyAll(spawnPos);
                SpawnInvulnerability(spawnPos);

                yield return new WaitForSeconds(_spawnWait);

            }    
            yield return new WaitForSeconds(_waveWait);
        }
    }

    private void SpawnEnemies()
    {
        if(_enemyCanSpawn)
        {
            _enemyCanSpawn = false;
            _isEnemySpawned = true;
            _deltaEnemyTime = _spawnEnemyTimer;
            Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

            GameObject GO = _objPool.GetObjectFromPool(TagStrings.ENEMY);
            GO.transform.position = spawnPos; 
        }

    }

    private void SpawnInvulnerability(Vector3 lastSpawnPos)
    {
        Vector3 spawnPos = Vector3.zero;
        int number = Utilities.RandomIntGenerator(1, 100);

        if(number >= 1 && number <= _chanceToSpawnInvulnerability)
        {
            //print("Did it: " + number);
            while(Utilities.IsCloseEnoughToOther(spawnPos, lastSpawnPos, 0.3f))
            {
                spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);
            }

            GameObject GO = _objPool.GetObjectFromPool(TagStrings.INVULNERABLE);
            GO.transform.position = spawnPos;
        }

    }

    private void SpawnDestroyAll(Vector3 lastSpawnPos)
    {
        Vector3 spawnPos = Vector3.zero;
        int number = Utilities.RandomIntGenerator(1, 100);

        if(number >= 1 && number <= _chanceToSpawnDestroyAll)
        {
            //print("Did it: " + number);
            while(Utilities.IsCloseEnoughToOther(spawnPos, lastSpawnPos, 0.3f))
            {
                spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

                //_isDestroyAllSpawned = true;
            }

            GameObject GO = _objPool.GetObjectFromPool(TagStrings.DESTROYALL);
            GO.transform.position = spawnPos;
        }
    }

    private void SpawnCoins(Vector3 lastSpawnPos)
    {
        Vector3 spawnPos = Vector3.zero;
        float farEnough = 3.0f;
        float dist = 0.0f;

        while (dist < farEnough)
        {
            spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);
            dist = Vector3.Distance(spawnPos, lastSpawnPos);
        }


        GameObject GO = _objPool.GetObjectFromPool(TagStrings.COIN);
        GO.transform.position = spawnPos;
    }

    private void CalculateScore()
    {
        _score = Mathf.Abs (_playDistance * _scoreMultiplier);
        //_scoreText.text = _score.ToString();;
    }

    private void CalculatePlayDistance()
    {
        _playDistance = Mathf.Abs(_gameSpeed * _playTime / 100);
        _distanceText.text = _playDistance.ToString("F1");
    }

    private void CalculateGameSpeed()
    {
        if(Mathf.RoundToInt(_playTime) % 100 == 0)
        {
            _gameSpeed -= 0.05f;
            _speedText.text = Mathf.Abs(_gameSpeed).ToString("F1");

        }
    }

    private void UpdateHazardDestroyed()
    {
        _destroyedHazards += 1;
        _killText.text = "Kills: " + _destroyedHazards.ToString();

    }

    private void UpdateCoinScore()
    {
        _coinScore += 1;
        _scoreText.text = "Score: " + _coinScore.ToString();
    }

    private void EnemyDestroyed()
    {
        _enemyCanSpawn = true;
        _isEnemySpawned = false;
        _destroyedHazards += 1;
        _killText.text = "Kills: " + _destroyedHazards.ToString(); 
        _deltaEnemyTime = _spawnEnemyTimer;
    }

    private void InvulnerabilityOn()
    {
        _isPlayerInvulnerable = true;
    }

    private void InvulnerabilityOff()
    {
        _isPlayerInvulnerable = false;
    }

    private void GameOver()
    {
        this.enabled = false;
        Destroy(this.gameObject);

        SceneManager.LoadScene("Game");
        //_lvlMan.LoadLevel("Start Menu");

        print("You died");
    }
	
}
