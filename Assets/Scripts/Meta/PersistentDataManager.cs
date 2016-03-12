using UnityEngine;
using System.Collections;

public class PersistentDataManager : MonoBehaviour {

    private static PersistentDataManager instance = null;

    void Awake()
    {
        if (instance != null) {
            Destroy (this.gameObject);
            //print ("Dupe PersistentDataManager self-destructing!");
        } else 
        {
            //print("we cool");
            instance = this;
            GameObject.DontDestroyOnLoad (this.gameObject);
        }   
    }


    public static void SaveFloatValue(string key, float value)
    {
        if(PlayerPrefs.HasKey(key))
        {
            float old = PlayerPrefs.GetFloat(key);
            if(IsNewScoreHigher(old, value))
            {
                PlayerPrefs.SetFloat(key, value);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(key, value);
        }

    }

    public static float LoadFloatValue(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            return PlayerPrefs.GetFloat(key);
        }
        else
        {
            return 0;    
        }

    }

    private static bool IsNewScoreHigher(float prevScore, float thisScore)
    {
        return (thisScore > prevScore) ? true : false;
    }
    
}
