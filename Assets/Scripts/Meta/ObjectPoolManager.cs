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

        DontDestroyOnLoad(this);

        if(_objPool == null)
        {
            _objPool = GetComponent<ObjectPooling>();
            _pools = new Dictionary<string, List<GameObject>>();
        }

    }

    public void CreatePool(int size, GameObject prefab, string poolName)
    {
        try
        {
            _pools.Add(poolName, _objPool.CreatePool(size, prefab));   
        } catch (System.Exception ex)
        {
            print(ex + " - " + poolName);
        }

    }

    public GameObject GetObjectFromPool(string poolName)
    {
        List<GameObject> curList = RetrieveGOList(poolName);
     
        if(HasDisabled(curList))
        {
            GameObject GO = _objPool.GetObjectFromPool(curList);
            return GO;
        }

        return null;
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

    private bool HasDisabled(List<GameObject> pool)
    {
        bool hasDisabled = false;
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject GO = pool[i];
            if(!GO.activeSelf)
            {
                hasDisabled = true;
                break;
            }
        }

        return hasDisabled;

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
