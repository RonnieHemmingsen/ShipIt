using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private float _minTimeBetweenBoltTokenSpawns;
    [SerializeField]
    private float _maxTimeBetweenBoltTokenSpawns;
    [SerializeField]
    private string[] _tokenArray;

    private GameBalancer _zen;
    private ObjectPoolManager _objPool;
    private GameManager _GM;
    private HazardStates _state;

    private List<Vector3> _enemyPositions;

    private float _deltaTimeBetweenAsteroids;
    private float _deltaTimeBetweenLaserEnemies;
    private float _deltaTimeBetweenBulletEnemies;
    private float _deltaTimeBetweenTokenSpawns;
    private float _deltaTimeBetweenBoltTokenSpawns;
    private int _currentNumberOfEnemiesOnScreen;


	void Awake () {
        _zen = FindObjectOfType<GameBalancer>();
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _GM = FindObjectOfType<GameManager>();
	}

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, Reset);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, Reset);
    }
        
    IEnumerator Start()
    {
        //print("Start Hazards");
        _state = HazardStates.Initialise;

        while(true)
        {
            if(!_GM.IsStartingGame && !_GM.IsWaitingForNewGame)
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
                    case HazardStates.BoltTokenSpawns:
                        Bolts();
                        break;
                    default:
                        break;
                }
            }
            yield return 0;
        }

    }

    private void Reset()
    {
        _state = HazardStates.Initialise;
    }


    private void Initialise()
    {
        _enemyPositions = new List<Vector3>();
        _deltaTimeBetweenAsteroids = _zen.TimeBeforeAsteroidsCanAppear;

        _state = HazardStates.Evaluate;
    }

    private void Evaluate()
    {
        _deltaTimeBetweenAsteroids -= Time.deltaTime;
        _deltaTimeBetweenBulletEnemies -= Time.deltaTime;
        _deltaTimeBetweenLaserEnemies -= Time.deltaTime;
        _deltaTimeBetweenTokenSpawns -= Time.deltaTime;
        _deltaTimeBetweenBoltTokenSpawns -= Time.deltaTime;

        DetermineNumberOfEnemiesAlive();
        RemoveEnemyPositions();

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
            && _currentNumberOfEnemiesOnScreen < _zen.MaxNumberOfEnemiesOnScreen
            && rand <= _zen.ChanceToSpawnBulletEnemy 
            && _zen.CanSpawnBulletEnemies)
        {
            _deltaTimeBetweenBulletEnemies = Random.Range(_minTimeBetweenBulletEnemySpawn, _maxTimeBetweenBulletEnemySpawn);
            _state = HazardStates.BulletEnemies;
        }

        if(_deltaTimeBetweenLaserEnemies <= 0 
            && _GM.LaserEnemiesAlive < _zen.MaxNumberOfLaserEnemies 
            && _currentNumberOfEnemiesOnScreen < _zen.MaxNumberOfEnemiesOnScreen
            && _zen.CanSpawnLaserEnemies 
            && rand <= _zen.ChanceToSpawnLaserEnemy)
        {
            _deltaTimeBetweenLaserEnemies = Random.Range(_minTimeBetweenLaserEnemySpawn, _maxTimeBetweenLaserEnemySpawn);
            _state = HazardStates.LaserEnemies;
        }

        if(_deltaTimeBetweenTokenSpawns <= 0 
            && _zen.CanSpawnTokens 
            && rand <= _zen.ChanceToSpawnToken)
        {
            _deltaTimeBetweenTokenSpawns = Random.Range(_minTimeBetweenTokenSpawns, _maxTimeBetweenTokenSpawns);
            _state = HazardStates.Tokens;
        }

        if(_deltaTimeBetweenBoltTokenSpawns <= 0 && _zen.CanSpawnTokens)
        {
            _deltaTimeBetweenBoltTokenSpawns = Random.Range(_minTimeBetweenBoltTokenSpawns, _maxTimeBetweenBoltTokenSpawns);
            _state = HazardStates.BoltTokenSpawns;
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

        GameObject GO = _objPool.GetObjectFromPool(ObjectStrings.HAZARD);
        if(GO != null)
        {
            GO.transform.position = spawnPos;
            _GM.AsteroidsAlive++;
        }

        _state = HazardStates.Evaluate;

    }

    private void LaserEnemies()
    {
        bool canSpawnHere = false;
        Vector3 spawnPos = Vector3.zero;

        while (!canSpawnHere) {
            spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);
            canSpawnHere = CanSpawnHere(spawnPos);
        }

        GameObject GO = _objPool.GetObjectFromPool(ObjectStrings.LASER_ENEMY);
        if(GO != null)
        {
            _enemyPositions.Add(spawnPos);
            GO.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);    
            _GM.LaserEnemiesAlive++;
        }

        _state = HazardStates.Evaluate;
    }

    private void BulletEnemies()
    {
        bool canSpawnHere = false;
        Vector3 spawnPos = Vector3.zero;

        while (!canSpawnHere) {
            spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);
            canSpawnHere = CanSpawnHere(spawnPos);
        }

        GameObject GO = _objPool.GetObjectFromPool(ObjectStrings.BULLET_ENEMY);
        if(GO != null)
        {
            _enemyPositions.Add(spawnPos);
            GO.transform.position = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);    
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
            }
        }

        _state = HazardStates.Evaluate;
    }

    private void Coins()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(ObjectStrings.COIN);
        if(GO != null)
        {
            GO.transform.position = spawnPos;    
        }
    }

    private void Bolts()
    {
        Vector3 spawnPos = new Vector3(Random.Range(-_spawnValues.x, _spawnValues.x), _spawnValues.y, _spawnValues.z);

        GameObject GO = _objPool.GetObjectFromPool(ObjectStrings.BOLT_TOKEN);
        if(GO != null)
        {
            GO.transform.position = spawnPos;    
        }
        _state = HazardStates.Evaluate;
    }

    private bool CanSpawnHere(Vector3 spawnPos)
    {
        bool farEnough = true;

//        if(_enemyPositions.Count == 0)
//        {
//            return farEnough;
//        }
//
//        for (int i = 0; i < _enemyPositions.Count -1; i++)
//        {
//            if(Utilities.DistanceLessThanValueToOther(spawnPos, _enemyPositions[i], 4))
//            {
//                //print("Too close: " + Vector3.Distance(spawnPos, _enemyPositions[i]));
//                farEnough = false;
//                break;
//            }
//            //print("Not close: " + Vector3.Distance(spawnPos, _enemyPositions[i]));
//
//        }

        return farEnough;
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

    private void DetermineNumberOfEnemiesAlive()
    {
        _currentNumberOfEnemiesOnScreen = _GM.BulletEnemiesAlive + _GM.LaserEnemiesAlive;
    }

    private void RemoveEnemyPositions()
    {
        if(_enemyPositions.Count > _zen.MaxNumberOfEnemiesOnScreen)
        {            
            _enemyPositions.RemoveRange(0, 1);
        }
    }

    public enum HazardStates
    {
        Initialise,
        Evaluate,
        Nothing,
        Asteroids,
        LaserEnemies,
        BulletEnemies,
        BoltTokenSpawns,
        Tokens
    }
}
