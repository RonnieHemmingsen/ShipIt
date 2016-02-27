using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayTutorial : MonoBehaviour {

    [SerializeField]
    private Button _tutorialButton;

    private GameManager _GM;

    void Awake()
    {
        _GM = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(transform.position.z < 6)
        {
            
            DiscoverSelf();
        }
    }

    private void DiscoverSelf()
    {
        if(tag == TagStrings.DESTROY_ALL && !_GM.HasDestroyTokenHelpBeenDisplayed)
        {
            print(transform.root.name + " - " + transform.root.tag);
            DisplayButton();
            _GM.HasDestroyTokenHelpBeenDisplayed = true;   
            PlayerPrefs.SetString(GameSettings.HAS_DESTROY_ALL_TOKEN_HELP_BEEN_DISPLAYED, "true");
        }
        else if(tag == TagStrings.INVULNERABLE && !_GM.HasShieldTokenHelpBeenDisplayed) 
        {
            print(transform.root.name + " - " + transform.root.tag);
            DisplayButton();
            _GM.HasShieldTokenHelpBeenDisplayed = true;
            PlayerPrefs.SetString(GameSettings.HAS_SHIELD_TOKEN_HELP_BEEN_DISPLAYED, "true");
        }
        else if(tag == TagStrings.LUDICROUS_SPEED && !_GM.HasSpeedTokenHelpBeenDisplayed)
        {
            print(transform.root.name + " - " + transform.root.tag);
            DisplayButton();
            _GM.HasSpeedTokenHelpBeenDisplayed = true;
            PlayerPrefs.SetString(GameSettings.HAS_SPEED_TOKEN_HELP_BEEN_DISPLAYED, "true");

        }
    }
	
    private void DisplayButton()
    {
        print("display button" + transform.name);

        SetDisplayPosition();
        _tutorialButton.gameObject.SetActive(true);
        _tutorialButton.onClick.AddListener(delegate {
            HideButton();
        });
        Time.timeScale = 0;
    }

    private void HideButton()
    {
        Time.timeScale = 1;
        _tutorialButton.gameObject.SetActive(false);
    }

    private void SetDisplayPosition()
    {
        Vector3 pos = Vector3.zero;

        if(transform.position.x >= 0)
        {
            pos = new Vector3(transform.position.x -5, transform.position.y, transform.position.z);
        }
        else
        {
            pos = new Vector3(transform.position.x +5, transform.position.y, transform.position.z);
        }

        Vector3 eulerRot = new Vector3(90, 0,0);
        _tutorialButton.transform.position = pos;
        _tutorialButton.transform.rotation = Quaternion.Euler(eulerRot);
    }
}
