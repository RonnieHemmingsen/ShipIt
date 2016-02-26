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
    private float _tilt = 5f;
    [SerializeField]
    private int _numberOfShots = 10;
    [SerializeField]
    private float _fireRate = 0.5f;
    [SerializeField]
    private float _jumpRate = 2f;
    [SerializeField]
    private GameObject _bolt;
    [SerializeField]
    private GameObject _boltSpawnPos;
    [SerializeField]
    private GameObject _invulnerableForceField;
    [SerializeField]
    private GameObject _ludicrousSpeedAfterburner;
    [SerializeField]
    private Boundary _boundary;

    private Rigidbody _rigidbody;
    private AudioSource _laserSound;
    private Animator _anim;
    private ObjectPoolManager _objPool;
    private GameManager _GM;

    private float _timeBetweenShots;
    private float _moveHorizontal;
    private float _currentVertical;
    private float _lastVertical;
    private bool _hasFiredTouch;
    private Vector2 _startSwipePosition;



    public float ShipSpeed
    {
        get { return _shipSpeed; }
        set { _shipSpeed = value; }
    }

    public float Tilt
    {
        get { return _tilt; }
        set { _tilt = value; }
    }

    public int NumberOfShots
    {
        get { return _numberOfShots; }
        set { _numberOfShots = value; }
    }
        
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _laserSound = GetComponent<AudioSource>();
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

    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.INVULNERABILITY_ON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITY_OFF, InvulnerabilityOff);
        EventManager.StopListening(EventStrings.ENGAGE_LUDICROUS_SPEED, EngageLudicrousSpeed);
        EventManager.StopListening(EventStrings.DISENGAGE_LUDICROUS_SPEED, DisengageLudicrousSpeed);
        
    }

    void Start()
    {
        _invulnerableForceField.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        _ludicrousSpeedAfterburner.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        _objPool.CreatePool(_numberOfShots, _bolt, _bolt.tag);
        _lastVertical = Input.acceleration.y;
        _currentVertical = Input.acceleration.y;
    }

    void FixedUpdate()
    {
        MoveControl();
        LudicrousSpeedControl();
        InvulnerabilityControl();
        DestroyAllControl();
    }
        
    #region controls
    private void MoveControl()
    {
        //#if UNITY_EDITOR
        _moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }

        //#elif UNITY_IOS
        _moveHorizontal = Input.acceleration.x;
        //#endif

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
        _anim.SetFloat("RightTurn", _moveHorizontal);
        _anim.SetFloat("LeftTurn", -0.1f);
        }
        else
        {
        _anim.SetFloat("RightTurn", -0.1f);

        _anim.SetFloat("LeftTurn", Mathf.Abs(_moveHorizontal));
        //print(Mathf.Abs(_moveHorizontal));
        }          
    }

    private void DestroyAllControl()
    {
        //#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.Z))
        {
            if(_GM.HasDestroyAllToken)
            {
                EventManager.TriggerEvent(EventStrings.GET_REKT);
            }
        }
        //#elif UNITY_IOS
        _currentVertical = Input.acceleration.y;
        float difference = _currentVertical - _lastVertical;
        if(difference > 0.5f && _GM.HasDestroyAllToken)
        {
            print("Difference: " + difference);
            EventManager.TriggerEvent(EventStrings.GET_REKT);
        }
        else
        {
            _lastVertical = _currentVertical;
        }
        //#endif
    }

    private void InvulnerabilityControl()
    {
        //#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            if(_GM.HasInvulnerabilityToken)
            {
                EventManager.TriggerEvent(EventStrings.INVULNERABILITY_ON);
            }
        }
        //#elif UNITY_IOS
        Touch[] touches = Input.touches;
        if(Input.touchCount == 2 && _GM.HasInvulnerabilityToken)
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
        //#endif
    }

    private void LudicrousSpeedControl()
    {
        //#if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            if(_GM.HasLudicrousSpeedToken) 
            {
                EventManager.TriggerEvent(EventStrings.ENGAGE_LUDICROUS_SPEED);
                EventManager.TriggerEvent(EventStrings.GET_REKT);
                EventManager.TriggerEvent(EventStrings.START_CAMERA_SHAKE);
            }
        }
        //#elif UNITY_IOS


        Touch[] touchy = Input.touches;
        //Shoot once per tap
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Began && !_hasFiredTouch)
        {
            _hasFiredTouch = true;
            Fire();
        }
        //reset on tap end
        if(Input.touchCount == 1 && touchy[0].phase == TouchPhase.Ended && _hasFiredTouch)
        {
            _hasFiredTouch = false;
        }

        //check if swipe has started -cache location
        if(Input.touchCount == 2 && touchy[0].phase == TouchPhase.Began && touchy[1].phase == TouchPhase.Began)
        {
            _startSwipePosition = touchy[0].position;
            print("Start Position: " + _startSwipePosition);
        }

        //Check if swipe has progressed, trigger ludicrous speed if appropriate
        if(Input.touchCount == 2 && touchy[0].phase == TouchPhase.Moved && touchy[1].phase == TouchPhase.Moved)
        {
            float distance = Vector2.Distance(touchy[0].deltaPosition, _startSwipePosition);
            print("Swipe Distance: " + distance);
            if(distance > 500 && _GM.HasLudicrousSpeedToken)
            {
                EventManager.TriggerEvent(EventStrings.ENGAGE_LUDICROUS_SPEED);
                EventManager.TriggerEvent(EventStrings.GET_REKT);
                EventManager.TriggerEvent(EventStrings.START_CAMERA_SHAKE);
            }
        }
        //#endif

    }
    #endregion

    private void InvulnerabilityOn()
    {
        Image im =  _invulnerableForceField.GetComponent<Image>();

        StartCoroutine(FadeInAndOut(1f, im, () =>
        {
         //Not a damned thing... this time.   
        }));   
    }

    private void InvulnerabilityOff()
    {
        Image im =  _invulnerableForceField.GetComponent<Image>();
        StartCoroutine(FadeInAndOut(0f, im, () => {
            //Dont actually need this anymore, but leaving it in 
            // so I dont have to fucking research next time.
            //_invulnerableForceField.SetActive(false);    
        }));

    }

    private void EngageLudicrousSpeed()
    {
        print("engage");
        Image im = _ludicrousSpeedAfterburner.GetComponent<Image>();
        StartCoroutine(FadeInAndOut(1, im, () => {}));
    }

    private void DisengageLudicrousSpeed()
    {
        print("disengage");
        Image im = _ludicrousSpeedAfterburner.GetComponent<Image>();
        StartCoroutine(FadeInAndOut(0, im, () => {}));
    }
        

    IEnumerator FadeInAndOut(float fadeToValue, Image im, Action onComplete)
    {
        
        im.CrossFadeAlpha(fadeToValue, GameSettings.CROSSFADE_ALPHA_VALUE, false);
        yield return new WaitForSeconds(GameSettings.CROSSFADE_ALPHA_VALUE);
        onComplete();

    }

    private void Fire()
    {
        if (Time.time > _timeBetweenShots)
        {
            GameObject go = _objPool.GetObjectFromPool(TagStrings.BOLT);
            go.transform.position = _boltSpawnPos.transform.position;
            _timeBetweenShots = Time.time + _fireRate;
            _laserSound.Play();
        }
    }
     
}
