using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

    [SerializeField]
    private float rotate = 5f;
    [SerializeField]
    private bool isClockwiseRotation = true;

    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () {
        
    }

    // Update is called once per frame
    void Update () {
        transform.Rotate(Vector3.up * rotate);
    }
}
