using UnityEngine;
using System.Collections;

public class DestroyByTime : MonoBehaviour {

    [SerializeField]
    private float lifetime;

    private ParticleSystem _shuriken;
	// Use this for initialization
	void Start () {
        _shuriken = GetComponent<ParticleSystem>();

        if(_shuriken != null)
        {
            Destroy(gameObject, _shuriken.duration);    
        }
        else
        {
            Destroy(gameObject, lifetime);
        }

	}
	
}
