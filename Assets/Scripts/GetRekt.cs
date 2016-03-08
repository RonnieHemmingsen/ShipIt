using UnityEngine;
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
        EventManager.TriggerStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, tag);   
        Instantiate(_explosion, transform.position, transform.rotation);
        _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
    }
}

