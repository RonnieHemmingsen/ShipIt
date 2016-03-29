using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LoadGame : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(2,LoadSceneMode.Single);
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        EventManager.TriggerEvent(GameSettings.UPDATE_ACTIVE_SCENE);

	}
	
}
