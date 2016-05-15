using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {

    private int _frameCount = 0;
    private float _deltaTime = 0f;
    private float _fps = 0f;
    private float _accum = 0f;
    private float _updateRate = 1f;
    private float _timeLeft;

    public float FPS
    {
        get { return _fps; }
        set { _fps = value; }
    }


	// Use this for initialization
	void Start () {
        _timeLeft = _updateRate;
	}
	
	// Update is called once per frame
	void Update () {
        _timeLeft -= Time.deltaTime;
        _accum += Time.timeScale / Time.deltaTime;
        ++_frameCount;

        if (_timeLeft <= 0f)
        {
            _fps = _accum / _frameCount;
            _timeLeft = _updateRate;
            _accum = 0f;
            _frameCount = 0;
        }
	}
}
