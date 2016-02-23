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
    private Boundary _boundary;

    private Rigidbody _rigidbody;
    private AudioSource _laserSound;
    private float _moveHorizontal;
    private float _moveVertical;
    private ObjectPoolManager _objPool;

    private float _timeBetweenShots;
    private float _timeBetweenJumps;
    private bool _canJump;
    private Animator _anim;


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
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>().GetComponent<ObjectPoolManager>();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.INVULNERABILITYON, InvulnerabilityOn);
        EventManager.StartListening(EventStrings.INVULNERABILITYOFF, InvulnerabilityOff);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.INVULNERABILITYON, InvulnerabilityOn);
        EventManager.StopListening(EventStrings.INVULNERABILITYOFF, InvulnerabilityOff);
        
    }

    void Start()
    {
        _invulnerableForceField.GetComponent<CanvasRenderer>().SetAlpha(0.0f);
        _objPool.CreatePool(_numberOfShots, _bolt, _bolt.tag);
        _canJump = true;
    }

    void FixedUpdate()
    {
        #if UNITY_EDITOR
        _moveHorizontal = Input.GetAxis("Horizontal");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        //#endif

        #elif UNITY_IOS
        _moveHorizontal = Input.acceleration.x;


        Touch[] touchy = Input.touches;

        if(Input.touchCount == 1)
        {
            Fire();
        }
        #endif

        Vector3 val = new Vector3(_moveHorizontal, 0 ,0);
        //Debug.Log(_moveHorizontal);
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
        
    void Update()
    {
        _timeBetweenJumps += Time.deltaTime;
        if(_timeBetweenJumps >= _jumpRate && !_canJump)
        {
            _canJump = true;
        }
    }

    private void InvulnerabilityOn()
    {
        _invulnerableForceField.SetActive(true);
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
        

    IEnumerator FadeInAndOut(float fadeToValue, Image im, Action onComplete)
    {
        
        im.CrossFadeAlpha(fadeToValue, GameSettings.CROSSFADE_APHA_VALUE, false);
        yield return new WaitForSeconds(GameSettings.CROSSFADE_APHA_VALUE);
        onComplete();

    }

    public void Fire()
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
