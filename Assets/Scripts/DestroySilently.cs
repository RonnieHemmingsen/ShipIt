using UnityEngine;
using System.Collections;

public class DestroySilently : MonoBehaviour {


    private ObjectPoolManager _objPool;
    private GameManager _GM;

	// Use this for initialization
	void Awake () {
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
	}
	
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == TagStrings.BULLET_ENEMY || other.tag == TagStrings.LASER_ENEMY)
        {
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
        }
    }
}
