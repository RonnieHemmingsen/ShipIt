using UnityEngine;
using System.Collections;
using System;

public class BulletEnemyScript : MonoBehaviour {

    [SerializeField]
    private GameObject _weaponArray;
    [SerializeField]
    private GameObject _bullet;

    private ObjectPoolManager _objPool;
    private EnemyMovement _movement;
    private bool _isAtLocation;
    private bool _isReadyToRetreat;
    private bool _isFiring;

    void Awake()
    {
        _movement = GetComponent<EnemyMovement>();
        _objPool = FindObjectOfType<ObjectPoolManager>();
    }

    void Start()
    {
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
        }

        if(_isAtLocation && _movement.TimeBeforeShooting < 0 && _movement.ShootingTime > 0)
        {
            //Shoot
            if(!_isFiring)
            {
                EventManager.TriggerEvent(EventStrings.START_BULLETENEMY_SHOOTING);
                _isFiring = true;    
            }

           
            _movement.ShootingTime -= Time.deltaTime;
            //print(4 + " - " +  _shootingTime);
        }

        if(_movement.ShootingTime < 0)
        {
            
            //stop shooting
            EventManager.TriggerEvent(EventStrings.STOP_BULLETENEMY_SHOOTING);
            _isReadyToRetreat = true;
            _isFiring = false;
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
                EventManager.TriggerStringEvent(EventStrings.ENEMY_OUT_OF_BOUNDS, tag);
                _objPool.ReturnObjectToPool(transform.tag, gameObject);
            }
        }
    }
}
