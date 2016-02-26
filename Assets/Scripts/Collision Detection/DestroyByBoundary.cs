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
        if(other.tag == TagStrings.DESTROY_ALL || other.tag == TagStrings.INVULNERABLE || other.tag == TagStrings.LUDICROUS_SPEED)
        {
            EventManager.TriggerStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, other.tag);
        }
        _objMan.ReturnObjectToPool(other.tag, other.gameObject);
    }
}
