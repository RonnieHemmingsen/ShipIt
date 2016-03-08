using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    [SerializeField]
    private Vector3 _spawnValues;
    [SerializeField]
    private float _minTimeBetweenAsteroidSpawn;
    [SerializeField]
    private float _maxTimeBetweenAsteroidSpawn;
    [SerializeField]
    private float _minTimeBetweenLaserEnemySpawn;
    [SerializeField]
    private float _maxTimeBetweenLaserEnemySpawn;
    [SerializeField]
    private float _minTimeBetweenBulletEnemySpawn;
    [SerializeField]
    private float _maxTimeBetweenBulletEnemySpawn;
    [SerializeField]
    private float _minTimeBetweenTokenSpawns;
    [SerializeField]
    private float _maxTimeBetweenTokenSpawns;
    [SerializeField]
    private string[] _tokenArray;

    private GameBalancer _zen;
    private ObjectPoolManager _objPool;
    private GameManager _GM;
    private HazardStates _state;

    private float _deltaTimeBetweenAsteroids;
    private float _deltaTimeBetweenLaserEnemies;
    private float _deltaTimeBetweenBulletEnemies;
    private float _deltaTimeBetweenTokenSpawns;


	void Awake () {
        _zen = FindObjectOfType<GameBalancer>();
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _GM = FindObjectOfType<GameManager>();
	}
	
    IEnumerator Start()
    {
        _deltaTimeBetweenAsteroids = _zen.TimeBeforeAsteroidsCanAppear;
        _deltaTimeBetweenBulletEnemies = _zen.TimeBeforeBulletEnemiesCanAppear;
        _deltaTimeBetweenLaserEnemies = _zen.TimeBeforeLaserEnemiesCanAppear;


        print("Start Hazards");
        _state = HazardStates.Initialise;

        while(true)
        {
            switch (_state)
            {
                case HazardStates.Initialise:
                    Initialise();
                    break;
                case HazardStates.Evaluate:
                    Evaluate();
                    break;
                case HazardStates.Nothing:
                    Nothing();
                    break;
                case HazardStates.Asteroids:
                    Asteroids();
                    break;
                case HazardStates.LaserEnemies:
                    LaserEnemies();
                    break;
                case HazardStates.BulletEnemies:
                    BulletEnemies();
                    break;
                case HazardStates.Tokens:
                    Tokens();
                    break;
                default:
                    break;
            }

            yield return 0;
        }

    }

    private void Initialise()
    {
        _state = HazardStates.Evaluate;
    }

    private void Evaluate()
    {
        _deltaTimeBetweenAsteroids -= Time.deltaTime;
        _deltaTimeBetweenBulletEnemies -= Time.deltaTime;
        _deltaTimeBetweenLaserEnemies -= Time.deltaTime;
        _deltaTimeBetweenTokenSpawns -= Time.deltaTime;



        float rand = Random.value;

        if (_deltaTimeBetweenAsteroids <= 0 
            && _GM.AsteroidsAlive < _zen.MaxNumberOfAsteroids)
        {
            Coins();
            _deltaTimeBetweenAsteroids = Random.Range(_minTimeBetweenAsteroidSpawn, _maxTimeBetweenAsteroidSpawn);
            _state = HazardStates.Asteroids;
        }

        if(_deltaTimeBetweenBulletEnemies <= 0 
            && _GM.BulletEnemiesAlive < _zen.MaxNumberOfBulletEnemies 
            && rand <= _zen.ChanceToSpawnBulletEnemy 
            && _zen.CanSpawnBulletEnemies)
        {
            _deltaTimeBetweenBulletEnemies = Random.Range(_minTimeBetweenBulletEnemySpawn, _maxTimeBetweenBulletEnemySpawn);
            _state = HazardStates.BulletEnemies;
        }

        if(_deltaTimeBetweenLaserEnemies <= 0 
            && _GM.LaserEnemiesAlive < _zen.MaxNumberOfLaserEnemies 
            && _zen.CanSpawnLaserEnemies && rand <= _zen.ChanceToSpawnLaserEnemy)
        {
            _deltaTimeBetweenLaserEnemies = Random.Range(_minTimeBetweenLaserEnemySpawn, _maxTimeBetweenLaserEnemySpawn);
            _state = HazardStates.LaserEnemies;
        }

        if(_deltaTimeBetweenTokenSpawns <= 0 
            && _zen.CanSpawnTokens && rand <= _zen.ChanceToSpawnToken)
        {
            _deltaTimeBetweenTokenSpawns = Random.Range(_minTimeBetweenTokenSpawns, _maxTimeBetweenTokenSpawns);
            _state = HazardStates.Tokens;
        }

        if(_zen.IsNothing)
        {
            _state = HazardStates.Nothing;
        }

    }

    private void Nothing()
    {
        _state = HazardStates.Evaluate;
    }

    private void Asteroids()
    {
        //print("Asteroids");
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(TagStrings.HAZARD);
        if(GO != null)
        {
            GO.transform.position = spawnPos;
            _GM.AsteroidsAlive++;
        }

        _state = HazardStates.Evaluate;

    }

    private void LaserEnemies()
    {
        
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(TagStrings.LASER_ENEMY);
        if(GO != null)
        {
            GO.transform.position = spawnPos;    
            _GM.LaserEnemiesAlive++;
        }

        _state = HazardStates.Evaluate;
    }

    private void BulletEnemies()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(TagStrings.BULLET_ENEMY);
        if(GO != null)
        {
            GO.transform.position = spawnPos;    
            _GM.BulletEnemiesAlive++;
        }

        _state = HazardStates.Evaluate;
    }

    private void Tokens()
    {
        
        string thisToken = GetRandomToken();
        if(CheckAgainstTokenList(thisToken))
        {
            AddTokenToAliveList(thisToken);
            Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

            GameObject GO = _objPool.GetObjectFromPool(thisToken);
            if(GO != null)
            {
                GO.transform.position = spawnPos;    
                _GM.BulletEnemiesAlive++;
            }
        }

        _state = HazardStates.Evaluate;
    }

    private void Coins()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(TagStrings.COIN);
        if(GO != null)
        {
            GO.transform.position = spawnPos;    
        }
    }

    private string GetRandomToken()
    {
        int rand = Random.Range(0, _tokenArray.Length);
        return _tokenArray[rand];
    }

    private void AddTokenToAliveList(string name)
    {
        _GM.AliveTokenList.Add(name);   
    }

    //Returns false if the token is already active
    private bool CheckAgainstTokenList(string name)
    {
        for (int i = 0; i < _GM.AliveTokenList.Count -1; i++)
        {
            if(name == _GM.AliveTokenList[i])
            {
                return false;

            }
        }

        return true;
    }

    public enum HazardStates
    {
        Initialise,
        Evaluate,
        Nothing,
        Asteroids,
        LaserEnemies,
        BulletEnemies,
        Tokens
    }
}
