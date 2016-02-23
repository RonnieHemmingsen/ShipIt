using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

    public List<GameObject> CreatePool(int size, GameObject prefab)
    {
        List<GameObject> list;
        Transform parentGo = CreateParentObject(prefab);

        list = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab);
            obj.SetActive(false);
            obj.transform.parent = parentGo;
            list.Add(obj);
        }

        return list;
    }

    public GameObject GetObjectFromPool(List<GameObject> list)
    {
        if(list.Count > 0)
        {
            GameObject obj = list[0];
            obj.SetActive(true);
            list.RemoveAt(0);
            return obj;
        }
        return null;
    }
        

    public List<GameObject> ReturnObjectToPool(List<GameObject> list, GameObject obj)
    {
        obj = ResetPosition(obj);
        list.Add(obj);
        obj.SetActive(false);

        return list;
    }

    public List<GameObject> ClearPool(List<GameObject> list)
    {
        for (int i = 0; i < list.Count -1; i++)
        {
            list.RemoveAt(i);
        }
        list = null; //TODO: the fuck?
        return list;
    }

    //Internal methods
    private Transform CreateParentObject(GameObject parent)
    {
        string name = parent.name;
        GameObject go = GameObject.Instantiate(new GameObject(name));
        return go.transform;
    }

    private GameObject ResetPosition(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        return obj;
    }
}
