using UnityEngine;
using System.Collections;

public class RemoveFromGame : MonoBehaviour {

    private ObjectPoolManager _objPool;
    private GameManager _GM;

	void Awake () {
        _GM = GameObject.FindObjectOfType<GameManager>();
        _objPool = GameObject.FindObjectOfType<ObjectPoolManager>();
	}
	
	// Update is called once per frame
	void Update () {
	
        //The jig is up, anything thats spawned after the
        //death of the player, is instantly returned to the pool
        //so as not to show up on screen.
        if(_GM.IsWaitingForNewGame)
        {
            //OBS: "this"-keyword not nessecary
            print("removed: " + name);
            _objPool.ReturnObjectToPool(tag, gameObject);
        }
	}
}
