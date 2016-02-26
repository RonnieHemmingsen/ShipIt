using UnityEngine;
using System.Collections;

public class LaserEnemyScript : MonoBehaviour {

    [SerializeField]
    private GameObject _laserBeam;

    private ObjectPoolManager _objPool;
    private EnemyMovement _movement;
    private Vector3 _laserPos;
    private bool _isAtLocation;
    private bool _isReadyToRetreat;
    private bool _isLaserActive;
    private GameObject _thisLaserBeam;

	// Use this for initialization
	void Awake () {
        
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _movement = GetComponent<EnemyMovement>();

	}

    void OnEnable()
    {
        _laserPos = new Vector3(_movement.GoToPosition.x + 0.05f, _movement.GoToPosition.y, _movement.GoToPosition.z + -6.5f);
    }
	
	// Update is called once per frame
	void Update () {
        
        if(Utilities.DistanceLessThanValueToOther(transform.position, _movement.GoToPosition, 0.3f) && !_isAtLocation)
        {
            //is not at location
            transform.position = Vector3.MoveTowards(transform.position, _movement.GoToPosition, _movement.Step);    
            //print("1");
        }
        else
        {
            //is at location
            _isAtLocation = true;
            //print(2);
        }

        if(_isAtLocation && _movement.TimeBeforeShooting > 0)
        {
            //Power up lazooors
            _movement.TimeBeforeShooting -= Time.deltaTime;
            //print(3 + " - " + _timeBeforeShooting);
        }

        if(_isAtLocation && _movement.TimeBeforeShooting < 0 && _movement.ShootingTime > 0)
        {
            //Shoot
            if(!_isLaserActive)
            {
                _thisLaserBeam = Instantiate(_laserBeam, _laserPos, Quaternion.identity) as GameObject;    
                _isLaserActive = true;
            }

            //_laserBeam.SetActive(true);
            _movement.ShootingTime -= Time.deltaTime;
            //print(4 + " - " +  _shootingTime);
        }

        if(_movement.ShootingTime < 0)
        {
            Destroy(_thisLaserBeam);
            _isLaserActive = false;
            //_laserBeam.SetActive(false);
            _isReadyToRetreat = true;
            //print(5);
        }

        if(_isReadyToRetreat && _movement.TimeBeforeRetreating > 0)
        {
            _movement.TimeBeforeRetreating -= Time.deltaTime;
            //print(6);

        }

        if(_isReadyToRetreat && _movement.TimeBeforeRetreating < 0)
        {
            //print(7);
            if(Utilities.DistanceLessThanValueToOther(transform.position, _movement.RetreatPosition, 0.3f))
            {
                transform.position = Vector3.MoveTowards(transform.position, _movement.RetreatPosition, _movement.Step);
            }
            else
            {
                print("i dont even know..");

                _objPool.ReturnObjectToPool(transform.tag, gameObject);
            }
        }


	}

}
