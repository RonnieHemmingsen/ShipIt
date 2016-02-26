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
        _objMan.ReturnObjectToPool(other.tag, other.gameObject);
    }
}
