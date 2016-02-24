using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileSwitcher : MonoBehaviour {

    [SerializeField]
    private GameObject[] _tileSets;
    [SerializeField]
    private Material _matOne;
    [SerializeField] 
    private Material _matTwo;
    [SerializeField]
    private Renderer _rend;

    private GameManager _GM;

    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();

    }
	// Use this for initialization
	void Start () {

        _rend.material = _matTwo;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
