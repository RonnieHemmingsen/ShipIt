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
    private float _chanceToSpawnDestroyAll = 0.01f;
    [SerializeField]
    private float _chanceToSpawnInvulnerability = 0.03f;
    [SerializeField]
    private float _chanceToSpawnEnemy = 0.08f;
    [SerializeField]
    private float _chanceToSpawnCoin = 1f;
    [SerializeField]
    private float _chanceToSpawnBigCoin = 0.02f;
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
    [SerializeField]
    private GameObject _bigCoin;


    private ObjectPoolManager _objPool;
    private List<Vector3> ItemSpawnPositions = new List<Vector3>();
    private List<Vector3> EnemySpawnPositions = new List<Vector3>();
    private float _score = 0;
    private float _playTime = 0;
    private float _playDistance = 0;
    private int _destroyedHazards = 0;
    private bool _isPlayerInvulnerable;
    private float _deltaInvulneTime;
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
        _objPool.CreatePool(100, _bigCoin, _bigCoin.tag);
        _objPool.CreatePool(20, _asteroid, _asteroid.tag);
        _objPool.CreatePool(10, _invulnePower, _invulnePower.tag);
        _objPool.CreatePool(5, _enemy, _enemy.tag);
        _objPool.CreatePool(10, _destroyAll, _destroyAll.tag);

        _killText.text = "Kills: 0";
        _distanceText.text = "Distance: 0";
        _speedText.text = "Speed: 5";
        _scoreText.text = "Score: 0";

        _deltaInvulneTime = _playerInvulnerableTimer;

        InvokeRepeating("SpawnDecider", 1.0f, .5f);
        StartCoroutine(SpawnWaves());

	}

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.PLAYERDEAD, GameOver);
        EventManager.StartListening(EventStrings.HAZARDKILL, UpdateHazardDestroyed); 
        EventManager.StartListening(EventStrings.COINGRAB, UpdateCoinScore);
        EventManager.StartListening(EventStrings.BIGCOINGRAB, UpdateBigCoinScore);
        EventManager.StartListening(EventStrings.ENEMYDESTROYED, EnemyDestroyed);
        EventManager.StartListening(EventStrings.INVULNERABILITYON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITYOFF, InvulnerabilityOff);
        
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.PLAYERDEAD, GameOver);
        EventManager.StopListening(EventStrings.HAZARDKILL, UpdateHazardDestroyed);
        EventManager.StopListening(EventStrings.COINGRAB, UpdateCoinScore);
        EventManager.StopListening(EventStrings.BIGCOINGRAB, UpdateBigCoinScore);
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


                yield return new WaitForSeconds(_spawnWait);

            }    
            yield return new WaitForSeconds(_waveWait);
        }
    }

    private void SpawnDecider()
    {
            float rand = Random.value;

        if(rand <= _chanceToSpawnCoin) 
        {
            SpawnStuff(TagStrings.COIN);    
        }
            
        if(rand <= _chanceToSpawnBigCoin)
        {
            SpawnStuff(TagStrings.BIGCOIN);
        }

        if (rand <= _chanceToSpawnEnemy) 
        {
        //print(rand + " <= " + _chanceToSpawnEnemy);
            SpawnStuff(TagStrings.ENEMY);  
        }

        if (rand <= _chanceToSpawnDestroyAll) 
        {
            
            SpawnStuff(TagStrings.DESTROYALL);    
        }

        if (rand <= _chanceToSpawnInvulnerability) 
        {
            SpawnStuff(TagStrings.INVULNERABLE);    
        }
    }

    private void SpawnStuff(string tag)
    {
        bool canSpawnHere = false;
        Vector3 spawnPos = Vector3.zero;


        while(!canSpawnHere)
        {
            spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

            if(EnemySpawnPositions.Count == 0)
            {
                EnemySpawnPositions.Add(spawnPos);
                canSpawnHere = true;
            }

            if(tag == TagStrings.ENEMY)
            {
                foreach (var item in EnemySpawnPositions)
                {
                    if(Utilities.DistanceLessThanValueToOther(spawnPos, item, 1.0f))
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
                foreach (var item in EnemySpawnPositions)
                {
                    if(Utilities.DistanceLessThanValueToOther(spawnPos, item, 0.3f))
                    {
                        canSpawnHere = true;
                        break;
                    }
                }
            }
        }

        if(tag == TagStrings.ENEMY)
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
	
    private void MaintainPositionLists(List<Vector3> list)
    {
        //print("List count : " +list.Count);

        if(list.Count < 5)
            return;

        list.RemoveAt(0);
    }
}
   
