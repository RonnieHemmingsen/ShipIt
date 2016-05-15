using UnityEngine;
using System.Collections;

public class GenericFaceDirection : MonoBehaviour {

    [SerializeField]
    private Transform _object;
    [SerializeField]
    private Vector3 _faceDirection;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        _object.LookAt(_faceDirection);
	}
}
