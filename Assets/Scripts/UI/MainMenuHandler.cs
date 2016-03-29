using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField]
    private float _timeBeforeGivingUp;
    [SerializeField]
    private GameObject _title;
    [SerializeField]
    private GameObject _BottomMenu;
    [SerializeField]
    private GameObject _FacebookMenu;
    [SerializeField]
    private GameObject _LoggedInMenu;
    [SerializeField]
    private GameObject _FacebookSpinner;

    private LoginStates _state;
    private Animator _anim;
    private bool _isSpinning;
    private bool _stopLoginTimer;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _LoggedInMenu.SetActive(false);
        _FacebookMenu.SetActive(false);
        _FacebookSpinner.SetActive(false);
        _state = LoginStates.Init;
    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.START_SPINNER, StartFBSpinner);
        EventManager.StartListening(GameSettings.STOP_SPINNER, StopFBSpinner);
        EventManager.StartListening(GameSettings.ONLINE_BUTTON_PRESSED, SetUserPressedOnlineButtonMarker);
        EventManager.StartListening(GameSettings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksMarker);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.START_SPINNER, StartFBSpinner);
        EventManager.StopListening(GameSettings.STOP_SPINNER, StopFBSpinner);
        EventManager.StopListening(GameSettings.ONLINE_BUTTON_PRESSED, SetUserPressedOnlineButtonMarker);
        EventManager.StopListening(GameSettings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksMarker);
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == GameSettings.MAIN_MENU_SCENE)
        {
            switch (_state)
            {
                case LoginStates.Init:
                    Init();
                    break;
                case LoginStates.Idle:
                    Idle();
                    break;
                case LoginStates.Offline:
                    Offline();
                    break;
                case LoginStates.UserPressedLogin:
                    UserPressedLogin();
                    break;
                case LoginStates.UserLoggedInToGameSparks:
                    UserLoggedInToGameSparks();
                    break;
                case LoginStates.LoginSequenceComplete:
                    LoginSequenceComplete();
                    break;
                case LoginStates.GetNewData:
                    GetNewData();
                    break;
                
                default:
                    break;
            }
        }

    }

    private void Init()
    {
        _FacebookSpinner.SetActive(true);
        EventManager.TriggerEvent(GameSettings.START_SPINNER);
        StartCoroutine(PlayerData.instance.CheckIfUserIsLoggedIn((response) => {
            if(response)
            {
                _state = LoginStates.GetNewData;

            }
            else
            {
                EventManager.TriggerEvent(GameSettings.STOP_SPINNER);
                _FacebookSpinner.SetActive(false);
                _state = LoginStates.Offline;
            }            
        }));
        _state = LoginStates.Idle;
    }

    private void Idle()
    {
        //nothing
    }

    private void Offline()
    {
        _FacebookMenu.SetActive(true);
        _LoggedInMenu.SetActive(false);
        _state = LoginStates.Idle;

    }

    private void UserPressedLogin()
    {
        
        _FacebookMenu.SetActive(false);
        StartFBSpinner();

        _state = LoginStates.Idle;
    }

    private void UserLoggedInToGameSparks()
    {
        StartCoroutine(PlayerData.instance.LoadData((response) => {
            print("all data should be loaded");
            _state = LoginStates.LoginSequenceComplete;
        }));
        _state = LoginStates.Idle;

    }

    private void LoginSequenceComplete()
    {
        StopFBSpinner();
        _LoggedInMenu.SetActive(true);
        print("logincomplete");
        EventManager.TriggerEvent(GameSettings.UPDATE_LOGGED_IN_MENU);

        _state = LoginStates.Idle;
    }

    private void GetNewData()
    {
        _FacebookMenu.SetActive(false);

        StartCoroutine(PlayerData.instance.LoadData((response) => {
            EventManager.TriggerEvent(AnimatorStrings.TRIGGER_FB_SPINNER_OFF);
            _LoggedInMenu.SetActive(true);   
            _FacebookSpinner.SetActive(false);
            EventManager.TriggerEvent(GameSettings.UPDATE_LOGGED_IN_MENU);
        }));

        _state = LoginStates.Idle;
    }


    private void SetUserPressedOnlineButtonMarker()
    {
        _state = LoginStates.UserPressedLogin;
    }

    private void SetUserLoggedInToGamesparksMarker()
    {
        _state = LoginStates.UserLoggedInToGameSparks;
    }

    private void SetSilentFacebookInitDoneMarker()
    {
        _state = LoginStates.GetNewData;
    }

    private void StartFBSpinner()
    {   
        if(!_isSpinning)
        {
            _isSpinning = true;
            _FacebookSpinner.SetActive(true);
            _anim.SetTrigger(AnimatorStrings.TRIGGER_FB_SPINNER_ON);    
        }
    }

    private void StopFBSpinner()
    {
        if(_isSpinning)
        {
            _isSpinning = false;
            _anim.SetTrigger(AnimatorStrings.TRIGGER_FB_SPINNER_OFF);   
            _FacebookSpinner.SetActive(false);
        }
    }

}

public enum LoginStates
{
    Init,
    Idle,
    Offline,
    UserPressedLogin,
    UserLoggedInToGameSparks,
    LoginSequenceComplete,
    GetNewData

}
