using UnityEngine;
using System.Collections;

public class DestroyByPosition : MonoBehaviour {

    [SerializeField]
    private Vector3 _killPosition;

    private ObjectPoolManager _objMan;

    void Awake()
    {
        _objMan = GameObject.FindObjectOfType<ObjectPoolManager>();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3.Distance(transform.position, _killPosition) < 1.0f)
        {
            print(string.Format("Bam! Dead. Position: {0}", transform.position));
        }
        else
        {
            //print(string.Format("kEpsilon value: {0} - Current distance: {1}", Vector3.kEpsilon, Vector3.Distance(transform.position, _killPosition)));
        }
	}
}
