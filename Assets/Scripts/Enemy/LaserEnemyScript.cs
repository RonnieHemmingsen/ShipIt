using UnityEngine;
using System.Collections;

public class LaserEnemyScript : MonoBehaviour {

    [SerializeField]
    private GameObject _laserBeam;

    private ObjectPoolManager _objPool;
    private EnemyMovement _movement;
    private bool _isAtLocation;
    private bool _isReadyToRetreat;
    private bool _isLaserActive;
    private GameObject _thisLaserBeam;

	// Use this for initialization
	void Awake () {
        
        _objPool = FindObjectOfType<ObjectPoolManager>();
        _movement = GetComponent<EnemyMovement>();


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
            //print(3 + " - " + _movement.ShootingTime);
        }

        if(_isAtLocation && _movement.TimeBeforeShooting < 0 && _movement.ShootingTime > 0)
        {
            //Shoot
            if(!_isLaserActive)
            {
                _laserBeam.SetActive(true);
                _isLaserActive = true;
            }

            //_laserBeam.SetActive(true);
            _movement.ShootingTime -= Time.deltaTime;
            //print(4 + " - " +  _shootingTime);
        }

        if(_movement.ShootingTime < 0)
        {
            _laserBeam.SetActive(false);
            _isLaserActive = false;
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
                EventManager.TriggerStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, tag);
                _objPool.ReturnObjectToPool(transform.tag, gameObject);
            }
        }


	}

}
