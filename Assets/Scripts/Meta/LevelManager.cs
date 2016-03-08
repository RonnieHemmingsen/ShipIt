using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

    private static LevelManager instance = null;

    void Awake()
    {
        if (instance != null) {
            Destroy (this.gameObject);
            print ("Dupe Levelmanager self-destructing!");
        } else 
        {
            print("we cool");
            instance = this;
            GameObject.DontDestroyOnLoad (this.gameObject);
        }
    }

    void Start()
    {
        //GameSparksHandler.AuthenticateUser("FB Player", "pw");
    }

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
        SceneManager.LoadScene(name, LoadSceneMode.Single);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

}
