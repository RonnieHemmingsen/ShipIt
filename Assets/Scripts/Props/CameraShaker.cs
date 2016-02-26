using UnityEngine;
using System.Collections;

public class CameraShaker : MonoBehaviour {

    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private float _shakeTime;
    [SerializeField]
    private float _shakeMagnitude;
    [SerializeField]
    private float _decreaseFactor;


    private float _deltaShakeTime; 
    private bool _canShake;
    void OnEnable()
    {
        EventManager.StartListening(EventStrings.START_CAMERA_SHAKE, StartShake);
        EventManager.StartListening(EventStrings.STOP_CAMERA_SHAKE, StopShake);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.START_CAMERA_SHAKE, StartShake);
        EventManager.StopListening(EventStrings.STOP_CAMERA_SHAKE, StopShake);
    }

    void Update()
    {
        if(_canShake)
        {
            _deltaShakeTime = _shakeTime;
            _mainCamera.transform.localPosition = Random.insideUnitSphere * _shakeMagnitude;    
        }

    }

    private void StartShake()
    {
        _canShake = true;
    }

    private void StopShake()
    {
        _canShake = false;
        while (_shakeTime > 0)
        {
            _mainCamera.transform.localPosition = Random.insideUnitSphere * _shakeMagnitude;
            _shakeTime -= Time.deltaTime * _decreaseFactor;    
        }

    }
}


//var camera : Camera; // set this via inspector
//var shake : float = 0;
//var shakeAmount : float = 0.7;
//var decreaseFactor : float = 1.0;
//
//function Update() {
//    if (shake > 0) {
//        Camera.transform.localPosition = Random.insideUnitSphere * shakeAmount;
//        shake -= Time.deltaTime * decreaseFactor;
//
//    } else {
//        shake = 0.0;
//    }
//}
