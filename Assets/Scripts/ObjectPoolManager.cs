using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ObjectPoolManager : MonoBehaviour {

    public static ObjectPoolManager instance;


    private Dictionary<string, List<GameObject>> _pools;
    private ObjectPooling _objPool;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            if(this != instance)
                Destroy(this.gameObject);
        }

        //DontDestroyOnLoad(this);

        if(_objPool == null)
        {
            _objPool = GetComponent<ObjectPooling>();
            _pools = new Dictionary<string, List<GameObject>>();
        }

    }

    public void CreatePool(int size, GameObject prefab, string poolName)
    {
        _pools.Add(poolName, _objPool.CreatePool(size, prefab));
    }

    public GameObject GetObjectFromPool(string poolName)
    {
        List<GameObject> curList = RetrieveGOList(poolName);
     
        GameObject GO = _objPool.GetObjectFromPool(curList);
        return GO;
    }

    public void ReturnObjectToPool(string poolName, GameObject obj)
    {
        List<GameObject> curList = RetrieveGOList(poolName);
        _objPool.ReturnObjectToPool(curList, obj);
    }

    public void ClearPool(string poolName)
    {
        List<GameObject> curList = RetrieveGOList(poolName);
        _objPool.ClearPool(curList);
        RemovePool(poolName);
    }

    private List<GameObject> RetrieveGOList(string poolName)
    {
        List<GameObject> curList = new List<GameObject>();
        //print(poolName);
        if (_pools.ContainsKey(poolName))
        {
            curList = _pools[poolName];
        }

        return curList;
    }

    private void RemovePool(string poolName)
    {
        if (_pools.ContainsKey(poolName))
        {
            _pools.Remove(poolName);
        }
    }
}
