using UnityEngine;
using System.Collections;

public class DestroyByBoundary : MonoBehaviour {

    private ObjectPoolManager _objMan;

    void Awake()
    {
        _objMan = GameObject.FindObjectOfType<ObjectPoolManager>();
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == TagStrings.DESTROY_ALL 
            || other.tag == TagStrings.INVULNERABLE 
            || other.tag == TagStrings.LUDICROUS_SPEED 
            || other.tag == TagStrings.BIG_COIN)
        {
            EventManager.TriggerStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, other.tag);
            _objMan.ReturnObjectToPool(other.tag, other.gameObject);

        }else if(other.tag == TagStrings.HAZARD)
        {
            EventManager.TriggerEvent(EventStrings.HAZARD_OUT_OF_BOUNDS);
            _objMan.ReturnObjectToPool(other.tag, other.gameObject);

        }else if(other.tag == TagStrings.BULLET_ENEMY || other.tag == TagStrings.LASER_ENEMY)
        {
            EventManager.TriggerStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, other.tag);
        }else
        {
        _objMan.ReturnObjectToPool(other.tag, other.gameObject);   
        }
    }
}
