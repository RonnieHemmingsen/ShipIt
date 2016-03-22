﻿using UnityEngine;
using System.Collections;

public class GetRekt : MonoBehaviour {

    [SerializeField]
    private GameObject _explosion;

    private ObjectPoolManager _objPool;
    void Awake()
    {
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.GET_REKT, Explode);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.GET_REKT, Explode);
    }

    private void Explode()
    {
        if(tag == ObjectStrings.HAZARD)
        {
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
        }

        if(tag == ObjectStrings.BULLET_ENEMY || tag == ObjectStrings.LASER_ENEMY)
        {
            EventManager.TriggerStringEvent(EventStrings.ENEMY_DESTROYED, tag);
        }

        Instantiate(_explosion, transform.position, transform.rotation);
        _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
    }
}

