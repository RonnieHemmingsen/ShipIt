using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[System.Serializable]
public class Boundary 
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float _shipSpeed = 5f;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private GameObject _boltSpawnPos;
    [SerializeField]
    private GameObject _invulnerableForceField;
    [SerializeField]
    private GameObject _ludicrousSpeedAfterburner;
    [SerializeField]
    private Boundary _boundary;
    [SerializeField]
    private AudioSource _engineSound;
    [SerializeField]
    private AudioSource _ludicrousSound;
    [SerializeField]
    private AudioSource _laserSound;

    private Rigidbody _rigidbody;
    private ObjectPoolManager _objPool;
    private GameManager _GM;
    private Animator _anim;
    private float _timeBetweenShots;
    private float _moveHorizontal;
    private float _currentVertical;
    private float _lastVertical;
    private bool _hasFiredTouch;
    private bool _isStartingGame = true;
    private Vector2 _startSwipePosition;
    private bool _shieldIsFading;

    public float ShipSpeed
    {
        get { return _shipSpeed; }
        set { _shipSpeed = value; }
    }
        

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
        _GM = GameObject.FindObjectOfType<GameManager>();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StartListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StartListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicrousSpeed);

        EventManager.StartListening(GameSettings.GAME_HAS_STARTED, GameStarted);

    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StopListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StopListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicrousSpeed);



        EventManager.StopListening(GameSettings.GAME_HAS_STARTED, GameStarted);
    }

    void Start()
    {
        _invulnerableForceField.GetComponent<CanvasRenderer>().SetAlpha(0f);
        _ludicrousSpeedAfterburner.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
    }

    void FixedUpdate()
    {
        if(!_isStartingGame)
        {
            MoveControl();
            LudicrousSpeedControl();
            ShieldControl();
            DestroyAllControl();
            //_engineSound.Play();    
        }

    }
        
    #region controls
    private void MoveControl()
    {
        #if UNITY_EDITOR
        _moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space) && CanFire())
        {
            Fire();
            EventManager.TriggerEvent(EventStrings.PLAYER_SHOOTS);

        }


        #elif UNITY_IOS
        _moveHorizontal = Input.acceleration.x;


        Touch[] touchy = Input.touches;
        //Shoot once per tap
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Began && !_hasFiredTouch && CanFire())
        {
            _hasFiredTouch = true;
            Fire();
            EventManager.TriggerEvent(EventStrings.PLAYER_SHOOTS);
        }
        //reset on tap end
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Ended && _hasFiredTouch)
        {
            _hasFiredTouch = false;
        }
        #endif

        Vector3 val = new Vector3(_moveHorizontal, 0 ,0);
        _rigidbody.velocity =  val * ShipSpeed;


        _rigidbody.position = new Vector3
        (
        Mathf.Clamp (_rigidbody.position.x, _boundary.xMin, _boundary.xMax),
        Mathf.Clamp (_rigidbody.position.y, _boundary.yMin, _boundary.yMax),
        Mathf.Clamp (_rigidbody.position.z, _boundary.zMin, _boundary.zMax)
        );


        if(_moveHorizontal >= 0)
        {
            //_anim.SetFloat("RightTurn", _moveHorizontal);
            //_anim.SetFloat("LeftTurn", -0.1f);
        }
        else
        {
            //_anim.SetFloat("RightTurn", -0.1f);

            //_anim.SetFloat("LeftTurn", Mathf.Abs(_moveHorizontal));
            //print(Mathf.Abs(_moveHorizontal));
        }          
    }

    private void DestroyAllControl()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_GM.HasDestroyAllToken)
            {
                EventManager.TriggerEvent(EventStrings.GET_REKT);
                EventManager.TriggerStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ObjectStrings.DESTROY_ALL);
            }
        }
        #elif UNITY_IOS
        _currentVertical = Input.acceleration.y;
        float difference = _currentVertical - _lastVertical;
        if(difference > 0.3f && _GM.HasDestroyAllToken)
        {
            print("Difference: " + difference);
            EventManager.TriggerEvent(EventStrings.GET_REKT);
            EventManager.TriggerStringEvent(EventStrings.TOKEN_OUT_OF_BOUNDS, ObjectStrings.DESTROY_ALL);
        }
        else
        {
            _lastVertical = _currentVertical;
        }
        #endif
    }

    private void ShieldControl()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(_GM.HasShieldToken)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_ON);
            }
        }
        #elif UNITY_IOS
        Touch[] touches = Input.touches;
        if(Input.touchCount == 2 && _GM.HasShieldToken)
        {
            bool leftSideTouch = false;
            bool rightSideTouch = false;
            foreach (var t in touches) 
            {
                if(t.position.x < Screen.width / 2)
                {
                    leftSideTouch = true;
                }
                if(t.position.x > Screen.width / 2)
                {
                    rightSideTouch = true;
                }
            }

            if(leftSideTouch == true && rightSideTouch == true)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_ON);   
            }
        }
        #endif
    }

    private void LudicrousSpeedControl()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(_GM.HasSpeedToken) 
            {
                _ludicrousSound.Play();
                EventManager.TriggerEvent(EventStrings.ENGAGE_LUDICROUS_SPEED);
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_ON);
                EventManager.TriggerEvent(EventStrings.GET_REKT);
            }
        }
        #elif UNITY_IOS


        //check if swipe has started -cache location
        Touch[] touchy = Input.touches;
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Began)
        {
            _startSwipePosition = touchy[0].position;
            print("Start Position: " + _startSwipePosition);
        }

        //Check if swipe has progressed, trigger ludicrous speed if appropriate
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Moved)
        {
            float distance = Vector2.Distance(touchy[0].deltaPosition, _startSwipePosition);
            print("Swipe Distance: " + distance);
            if(distance > 500 && _GM.HasSpeedToken)
            {
                EventManager.TriggerEvent(EventStrings.ENGAGE_LUDICROUS_SPEED);
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_ON);
                EventManager.TriggerEvent(EventStrings.GET_REKT);
            }
        }
        #endif

    }
    #endregion



    private bool CanFire()
    {
        return _GM.CurrentNumberOfBolts > 0 ? true : false;
    }


    #region effect graphics
    private void InvulnerabilityOn()
    {
        Image im =  _invulnerableForceField.GetComponent<Image>();
        _shieldIsFading = true;
        _invulnerableForceField.SetActive(true);   
        StartCoroutine(FadeInAndOutOverTime(1f, 0.3f, im, _GM.PlayerShieldTime));

    }

    private void InvulnerabilityOff()
    {
        _shieldIsFading = false;
        _invulnerableForceField.SetActive(false);   
//        Image im =  _invulnerableForceField.GetComponent<Image>();
//        StartCoroutine(Utilities.FadeInAndOut(0f, im, () => {
//            //Dont actually need this anymore, but leaving it in 
//            // so I dont have to fucking research next time.
//            //_invulnerableForceField.SetActive(false);    
//        }));
    }

    private void EngageLudicrousSpeed()
    {
        print("engage");
        EventManager.TriggerEvent(EventStrings.START_CAMERA_SHAKE);
        Image im = _ludicrousSpeedAfterburner.GetComponent<Image>();
        StartCoroutine(Utilities.FadeInAndOut(1, im, () => {}));
    }

    private void DisengageLudicrousSpeed()
    {
        print("disengage");
        EventManager.TriggerEvent(EventStrings.STOP_CAMERA_SHAKE);
        Image im = _ludicrousSpeedAfterburner.GetComponent<Image>();
        StartCoroutine(Utilities.FadeInAndOut(0, im, () => {}));
    }
    #endregion

    private void Fire()
    {
        if (Time.time > _timeBetweenShots)
        {
            GameObject go = _objPool.GetObjectFromPool(ObjectStrings.BOLT);
            go.transform.position = _boltSpawnPos.transform.position;
            _timeBetweenShots = Time.time + _fireRate;
            _laserSound.Play();
        }
    }

    private IEnumerator FadeInAndOutOverTime(float maxValue, float minValue, Image im, float fadeTime)
    {
        float time = 0;
        float deltaCrossfade = 0;
        bool fadeIn = true;

        do
        {
            time += Time.deltaTime;
            deltaCrossfade += Time.deltaTime;

            if (fadeIn) {
                im.CrossFadeAlpha(maxValue, GameSettings.CROSSFADE_ALPHA_VALUE, false);        
            }else
            {
                im.CrossFadeAlpha(minValue, GameSettings.CROSSFADE_ALPHA_VALUE, false);
            }


            if(deltaCrossfade >= GameSettings.CROSSFADE_ALPHA_VALUE)
            {
                fadeIn = !fadeIn;
                deltaCrossfade = 0;
            }

            yield return new WaitForSeconds(0.03f);

        } while (time <= fadeTime && _shieldIsFading);

    }

    private void GameStarted()
    {
        _isStartingGame = false;
    }

     
}
