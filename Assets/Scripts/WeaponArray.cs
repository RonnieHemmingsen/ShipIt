using UnityEngine;
using System.Collections;

public class WeaponArray : MonoBehaviour {
    
    [SerializeField]
    private float _timeBetweenShots;
    [SerializeField]
    private float _bulletSpeed;
    [SerializeField]
    public GameObject[] _pipes;


    private ObjectPoolManager _objPool;
    private bool _canFireWeapon;
    private string _uniqueId;

    void Awake()
    {
        _objPool = FindObjectOfType<ObjectPoolManager>();
    }

    void OnEnable()
    {
        EventManager.StartListening(EventStrings.START_BULLETENEMY_SHOOTING, StartShooting);
        EventManager.StartListening(EventStrings.STOP_BULLETENEMY_SHOOTING, StopShooting);
    }

    void OnDisable()
    {
        EventManager.StopListening(EventStrings.START_BULLETENEMY_SHOOTING, StartShooting);
        EventManager.StopListening(EventStrings.STOP_BULLETENEMY_SHOOTING, StopShooting);
    }


	// Update is called once per frame
	void Update () {
	
	}

    private void StartShooting()
    {
        _canFireWeapon = true;
        StartCoroutine(FireWeapon());
    }

    private void StopShooting()
    {
        _canFireWeapon = false;   
    }


    private IEnumerator FireWeapon()
    {
        while (_canFireWeapon)
        {
            for (int i = 0; i < _pipes.Length; i++)
            {
                if(_pipes[i].activeSelf)
                {
                    GameObject go = _objPool.GetObjectFromPool(TagStrings.BULLET);
                    go.transform.position = _pipes[i].transform.position;
                    print(go.transform.rotation.eulerAngles);

                    Vector3 rot = _pipes[i].transform.rotation.eulerAngles;

                    go.transform.rotation = Quaternion.Euler(rot);
                    Rigidbody rigid = go.GetComponent<Rigidbody>(); 
                    rigid.velocity = go.transform.forward * _bulletSpeed;

                    //print("pipe: " + i + " - Rotation: " +  _pipes[i].transform.rotation.eulerAngles + "bullet rotation: " + go.transform.localRotation.eulerAngles);
                    //yield return new WaitForSeconds(0.01f);
                }

            }
            yield return new WaitForSeconds(_timeBetweenShots);

        }
    }
}
