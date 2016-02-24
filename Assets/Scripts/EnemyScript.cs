using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed = 5.0f;
    [SerializeField]
    private float _timeBeforeShooting = 2.0f;
    [SerializeField]
    private float _shootingTime = 5.0f;
    [SerializeField]
    private float _timeBeforeRetreating = 1.0f;
    [SerializeField]
    private GameObject _laserBeam;
    [SerializeField]
    private Vector3 _targetPos = new Vector3(5, 0, 8);

    private GameManager _GM;
    private ObjectPoolManager _objPool;
    private float _step;
    private Vector3 _gotoPos;
    private Vector3 _retreatPos;
    private Vector3 _laserPos;
    private bool _isAtLocation;
    private bool _isReadyToRetreat;
    private bool _isLaserActive;
    private GameObject _thisLaserBeam;

	// Use this for initialization
	void Start () {

        _GM = FindObjectOfType<GameManager>();
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _step = _moveSpeed * Time.deltaTime;
	}

    void OnEnable()
    {
        _gotoPos = new Vector3(Random.Range(-_targetPos.x, _targetPos.x), _targetPos.y, _targetPos.z);
        _retreatPos = new Vector3(_gotoPos.x, _gotoPos.y, _gotoPos.z + 3.0f);
        _laserPos = new Vector3(_gotoPos.x + 0.05f, _gotoPos.y, _gotoPos.z + -6.5f);


    }
	
	// Update is called once per frame
	void Update () {
        
        if(Utilities.DistanceLessThanValueToOther(transform.position, _gotoPos, 0.3f) && !_isAtLocation)
        {
            //is not at location
            transform.position = Vector3.MoveTowards(transform.position, _gotoPos, _step);    
            //print("1");
        }
        else
        {
            //is at location
            _isAtLocation = true;
            //print(2);
        }

        if(_isAtLocation && _timeBeforeShooting > 0)
        {
            //Power up lazooors
            _timeBeforeShooting -= Time.deltaTime;
            //print(3 + " - " + _timeBeforeShooting);
        }

        if(_isAtLocation && _timeBeforeShooting < 0 && _shootingTime > 0)
        {
            //Shoot
            if(!_isLaserActive)
            {
                _thisLaserBeam = Instantiate(_laserBeam, _laserPos, Quaternion.identity) as GameObject;    
                _isLaserActive = true;
            }

            //_laserBeam.SetActive(true);
            _shootingTime -= Time.deltaTime;
            //print(4 + " - " +  _shootingTime);
        }

        if(_shootingTime < 0)
        {
            Destroy(_thisLaserBeam);
            _isLaserActive = false;
            //_laserBeam.SetActive(false);
            _isReadyToRetreat = true;
            //print(5);
        }

        if(_isReadyToRetreat && _timeBeforeRetreating > 0)
        {
            _timeBeforeRetreating -= Time.deltaTime;
            //print(6);

        }

        if(_isReadyToRetreat && _timeBeforeRetreating < 0)
        {
            //print(7);
             if(Utilities.DistanceLessThanValueToOther(transform.position, _retreatPos, 0.3f))
            {
                transform.position = Vector3.MoveTowards(transform.position, _retreatPos, _step);
            }
            else
            {
                print("i dont even know..");

                _objPool.ReturnObjectToPool(transform.tag, gameObject);
            }
        }


	}

}
