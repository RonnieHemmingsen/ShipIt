using UnityEngine;
using System.Collections;

public class TileScroller : MonoBehaviour {

    [SerializeField]
    private float _scrollSpeed;
    [SerializeField]
    private float _tileSizeZ;

    private Vector3 _startPosition;

	// Use this for initialization
	void Start () {
        _startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float newPos = Mathf.Repeat(Time.time * _scrollSpeed, _tileSizeZ);
        transform.position = _startPosition + Vector3.forward *newPos;
	}
}
