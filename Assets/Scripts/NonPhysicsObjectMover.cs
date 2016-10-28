using UnityEngine;
using System.Collections;

public class NonPhysicsObjectMover : MonoBehaviour {

    [SerializeField]
    private float _moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(transform.forward * Time.deltaTime * _moveSpeed);
	
	}
}
