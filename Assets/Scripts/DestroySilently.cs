using UnityEngine;
using System.Collections;

public class DestroySilently : MonoBehaviour {


    private ObjectPoolManager _objPool;
    private EnemyMovement _movement;

	// Use this for initialization
	void Awake () {
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
        _movement = GetComponentInParent<EnemyMovement>();
	}
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == ObjectStrings.BULLET_ENEMY || other.tag == ObjectStrings.LASER_ENEMY)
        {
            if (Utilities.DistanceLessThanValueToOther(transform.position, _movement.GoToPosition, 0.5f))
            {                
                _objPool.ReturnObjectToPool(gameObject.tag, gameObject);                
            }

        }
    }
}
