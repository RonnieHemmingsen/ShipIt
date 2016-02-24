using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _playerExplosion;

    private ObjectPoolManager _objPool;
    private GameManager _GM;

    void Awake()
    {
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>().GetComponent<ObjectPoolManager>();
        _GM = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {

        //print("This: " + this.tag + " - Other: " + other.tag);
        //Laser hits a hazzard
        if(other.tag == TagStrings.BOLT && this.tag == TagStrings.HAZARD)
        {
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(other.tag, other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARDKILL);
        }

        //Player hits hazzard, both are dead.
        if(other.tag == TagStrings.PLAYER && !_GM.IsPlayerInvulnerable && this.tag == TagStrings.HAZARD && !_GM.DebugInvulne)
        {
            
            print("Death By Hazard");
            Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYERDEAD);

        }

        //Player hits enemy laser and is fucking dead.
        if(other.tag == TagStrings.PLAYER && !_GM.IsPlayerInvulnerable && this.tag == TagStrings.ENEMYBOLT && !_GM.DebugInvulne)
        {
         
            print("Death by laser");
            Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYERDEAD);
        }

        //Invulnerable player hits hazard
        if(other.tag == TagStrings.PLAYER && _GM.IsPlayerInvulnerable)
        {
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARDKILL);
        }

        //Player grabs invulnerable powerup
        if(this.tag == TagStrings.INVULNERABLE && other.tag == TagStrings.PLAYER)
        {
            print(this.tag);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.INVULNERABILITYON);
        }

        //Player grabs coin
        if(this.tag == TagStrings.COIN && other.tag == TagStrings.PLAYER)
        {
            EventManager.TriggerEvent(EventStrings.COINGRAB);
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
        }

        //player grabs big coin
        if(this.tag == TagStrings.BIGCOIN && other.tag == TagStrings.PLAYER)
        {
            EventManager.TriggerEvent(EventStrings.BIGCOINGRAB);
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            
        }

        //Enemy destroyed by player bolt
        if(this.tag == TagStrings.ENEMY && other.tag == TagStrings.BOLT)
        {
            EventManager.TriggerEvent(EventStrings.ENEMYDESTROYED);
            Instantiate(_explosion, transform.position, Quaternion.identity);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
        }

        // Player Grabs DestroyAll
        if(this.tag == TagStrings.DESTROYALL && other.tag == TagStrings.PLAYER)
        {
            print("DestroyAll");

            var hazards = GameObject.FindGameObjectsWithTag(TagStrings.HAZARD);
            foreach (var hazard in hazards)
            {
                if(hazard.activeSelf == true)
                {
                    ExplodeAThing(hazard);
                }

            }

            Instantiate(_explosion, transform.position, Quaternion.identity);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
        }
    }

    private void ExplodeAThing(GameObject hazard)
    {
        Instantiate(_explosion, hazard.transform.position, hazard.transform.rotation);
        _objPool.ReturnObjectToPool(hazard.tag, hazard);
    }
}
