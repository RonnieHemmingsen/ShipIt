using UnityEngine;
using System.Collections;

public class RandomRotater : MonoBehaviour {

    [SerializeField]
    private float tumble = 5f;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

	// Use this for initialization
	void Start () {
        _rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
