using UnityEngine;
using System.Collections;

public class PlayerEnterGame : MonoBehaviour {

    [SerializeField]
    private Vector3 startPos;
    [SerializeField]
    private Vector3 endPos;
    [SerializeField]
    private float speed;


	// Use this for initialization
	void Start () {
        StartCoroutine(StartingGame());
	}
	
    private IEnumerator StartingGame()
    {
        bool moving = false;
        float time = 0;

        EventManager.TriggerEvent(EventStrings.ENGAGE_LUDICROUS_SPEED);
        yield return new WaitForSeconds(1f);

        if(!moving)
        {
            moving = true;
            while (Utilities.DistanceLessThanValueToOther(transform.position, endPos, 0.1f) && time < 1f)
            {
                time += Time.deltaTime;
                transform.position = Vector3.Lerp(startPos, endPos, time); 
                yield return 0;
            }
            moving = false;
        }

        EventManager.TriggerEvent(EventStrings.DISENGAGE_LUDICROUS_SPEED);
        yield return new WaitForSeconds(0.5f);
        EventManager.TriggerEvent(GameSettings.GAME_STARTED);
        


            


    }


}
