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
        _GM = GameObject.FindObjectOfType<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {

        //print("This: " + this.tag + " - Other: " + other.tag);
        //Player bolt hits a hazzard
        if(other.tag == ObjectStrings.BOLT && this.tag == ObjectStrings.HAZARD)
        {
            Instantiate(_explosion, transform.position, transform.rotation);
            _objPool.ReturnObjectToPool(other.tag, other.gameObject);
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
        }

        //Enemy bullet hits a hazard
        if(other.tag == ObjectStrings.HAZARD && tag == ObjectStrings.BULLET)
        {
            ExplodeAThing(other.gameObject);
            ExplodeAThing(gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);

        }

        //Player hits hazard, both are dead.
        if(other.tag == ObjectStrings.PLAYER && !_GM.IsPlayerShielded && this.tag == ObjectStrings.HAZARD && !_GM.DebugInvulne)
        {
            
            print("Death By Hazard");
            ExplodeAThing(gameObject);
            Destroy(other.gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);

        }

        //Player hits an enemy laser, and is having a bad day.
        if(other.tag == ObjectStrings.PLAYER && !_GM.IsPlayerShielded && tag == ObjectStrings.ENEMY_BOLT && !_GM.DebugInvulne)
        {
            print("Death by laser");
            ExplodeAThing(gameObject);
            Destroy(other.gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);
        }


        //Player hits a bullet and is hard pressed for luck
        if(other.tag == ObjectStrings.PLAYER && !_GM.IsPlayerShielded && tag == ObjectStrings.BULLET && !_GM.DebugInvulne)
        {
            print("Death by bullet");
            ExplodeAThing(gameObject);
            Destroy(other.gameObject);
            EventManager.TriggerEvent(EventStrings.PLAYER_DEAD);
        }

        //Invulnerable player hits hazard
        if(other.tag == ObjectStrings.PLAYER && _GM.IsPlayerShielded)
        {
            ExplodeAThing(gameObject);
            EventManager.TriggerEvent(EventStrings.HAZARD_KILL);
        }

        //Player grabs coin
        if(tag == ObjectStrings.COIN && other.tag == ObjectStrings.PLAYER)
        {
            _objPool.GetObjectFromPool(ObjectStrings.TWEEN_TEXT_OUT);
            EventManager.TriggerObjectEvent(EventStrings.TEXT_TWEEN, gameObject);
            EventManager.TriggerEvent(EventStrings.GRAB_COIN);
            ExplodeAThing(gameObject);
        }

        //player grabs big coin
        if(tag == ObjectStrings.BIG_COIN && other.tag == ObjectStrings.PLAYER)
        {
            HouseKeeping();
            EventManager.TriggerEvent(EventStrings.GRAB_BIG_COIN);
            ExplodeAThing(gameObject);
            
        }

        //player picks up bullet
        if(tag == ObjectStrings.BOLT_TOKEN && other.tag == ObjectStrings.PLAYER)
        {
            HouseKeeping();
            EventManager.TriggerEvent(EventStrings.GRAB_BOLT_TOKEN);
            ExplodeAThing(gameObject);

        }

        //Enemy destroyed by player bolt
        if((tag == ObjectStrings.LASER_ENEMY || tag == ObjectStrings.BULLET_ENEMY) && other.tag == ObjectStrings.BOLT)
        {
            EventManager.TriggerStringEvent(EventStrings.ENEMY_DESTROYED, tag);
            ExplodeAThing(gameObject);
        }

        // Player Grabs DestroyAll
        if(this.tag == ObjectStrings.DESTROY_ALL && other.tag == ObjectStrings.PLAYER)
        {
            print(tag);
            HouseKeeping();
            EventManager.TriggerEvent(EventStrings.GRAB_DESTROY_ALL_TOKEN);
            ExplodeAThing(gameObject);
        }

        //Player grabs invulnerable powerup
        if(tag == ObjectStrings.INVULNERABLE && other.tag == ObjectStrings.PLAYER)
        {
            print(tag);
            HouseKeeping();
            _objPool.ReturnObjectToPool(gameObject.tag, gameObject);
            EventManager.TriggerEvent(EventStrings.GRAB_INVUNERABILITY_TOKEN);
        }

        //Grab ludicrous speed token
        if(this.tag == ObjectStrings.LUDICROUS_SPEED && other.tag == ObjectStrings.PLAYER)
        {
            print(tag);
            HouseKeeping();
            EventManager.TriggerEvent(EventStrings.GRAB_LUDICROUS_SPEED_TOKEN);
            ExplodeAThing(this.gameObject);
        }
    }

    private void HouseKeeping()
    {
        _objPool.GetObjectFromPool(ObjectStrings.TWEEN_TEXT_OUT);
        EventManager.TriggerObjectEvent(EventStrings.TEXT_TWEEN, gameObject);
        EventManager.TriggerEvent(EventStrings.SPEED_DECREASE);
    }

    private void ExplodeAThing(GameObject obj)
    {
        Instantiate(_explosion, obj.transform.position, obj.transform.rotation);
        _objPool.ReturnObjectToPool(obj.tag, obj);
    }
}
