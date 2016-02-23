using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

    [SerializeField]
    private float _speed = 10;

    private Rigidbody _rigidbody;
    private GameManager _GM;

    public float Speed
    {
        get { return _speed; }
        set { _speed = value; }
    }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _GM = FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

	// Use this for initialization
	void OnEnable () {
        if(transform.tag == "Hazard")
        {
            _rigidbody.velocity = transform.forward * _GM.GameSpeed; //speed afhængig af GM.
        }
        else
        {
            _rigidbody.velocity = transform.forward * _speed; //speed sat på GO.
        }
	}
}
