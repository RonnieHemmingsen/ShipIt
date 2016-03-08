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
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
        _GM = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {

        //print("This: " + this.tag + " - Other: " + other.tag);
        //Player bolt hits a hazzard
        if(other.tag == TagStrings.BOLT && this.tag == TagStrings.HAZARD)
        {
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(other.tag, other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
        }

        //Enemy bullet hits a hazard
        if(other.tag == TagStrings.HAZARD && tag == TagStrings.BULLET)
        {
            ExplodeAThing(other.gameObject);
            ExplodeAThing(gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);

        }

        //Player hits hazard, both are dead.
        if(other.tag == TagStrings.PLAYER && !_GM.IsPlayerShielded && this.tag == TagStrings.HAZARD && !_GM.DebugInvulne)
        {
            
            print("Death By Hazard");
            Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerStringEvent(EventStrings.REMOVE_FROM_ALIVE_LIST, tag);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);

        }

        //Player hits enemy laser and is fucking dead.
        if(other.tag == TagStrings.PLAYER && !_GM.IsPlayerShielded && tag == TagStrings.ENEMY_BOLT && !_GM.DebugInvulne)
        {
         
            print("Death by laser");
            Instantiate(_explosion, transform.position, transform.rotation);
            Destroy(other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);
        }

        //Player hits a bullet and is hard pressed for luck
        if(other.tag == TagStrings.PLAYER && !_GM.IsPlayerShielded && tag == TagStrings.BULLET && !_GM.DebugInvulne)
        {
            print("Death by bullet");
            ExplodeAThing(gameObject);
            Destroy(other.gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);
        }

        //Invulnerable player hits hazard
        if(other.tag == TagStrings.PLAYER && _GM.IsPlayerShielded)
        {
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
        }

        //Player grabs invulnerable powerup
        if(tag == TagStrings.INVULNERABLE && other.tag == TagStrings.PLAYER)
        {
            print(tag);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.GRAB_INVUNERABILITY_TOKEN);
        }

        //Player grabs coin
        if(tag == TagStrings.COIN && other.tag == TagStrings.PLAYER)
        {
            EventManager.TriggerEvent(EventStrings.GRAB_COIN);
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
        }

        //player grabs big coin
        if(tag == TagStrings.BIG_COIN && other.tag == TagStrings.PLAYER)
        {
            EventManager.TriggerEvent(EventStrings.GRAB_BIG_COIN);
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            
        }

        //Enemy destroyed by player bolt
        if((tag == TagStrings.LASER_ENEMY || tag == TagStrings.BULLET_ENEMY) && other.tag == TagStrings.BOLT)
        {
            EventManager.TriggerEvent(EventStrings.ENEMY_DESTROYED);
            ExplodeAThing(gameObject);
        }

        // Player Grabs DestroyAll
        if(this.tag == TagStrings.DESTROY_ALL && other.tag == TagStrings.PLAYER)
        {
            print("DestroyAll");

            EventManager.TriggerEvent(EventStrings.GRAB_DESTROY_ALL_TOKEN);
            ExplodeAThing(gameObject);
        }

        //Grab ludicrous speed token
        if(this.tag == TagStrings.LUDICROUS_SPEED && other.tag == TagStrings.PLAYER)
        {
            print(tag);
            EventManager.TriggerEvent(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN);
            ExplodeAThing(this.gameObject);
        }
    }

    private void ExplodeAThing(GameObject hazard)
    {
        Instantiate(_explosion, hazard.transform.position, hazard.transform.rotation);
        _objPool.ReturnObjectToPool(hazard.tag, hazard);
    }
}
