using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField]
    private float _timeBeforeGivingUp;
    [SerializeField]
    private GameObject _title;
    [SerializeField]
    private GameObject _facebookMenu;
    [SerializeField]
    private GameObject _offlineMenu;
    [SerializeField]
    private GameObject _loggedInMenu;
    [SerializeField]
    private GameObject _facebookSpinner;
    [SerializeField] 
    private RectTransform _profileRectTransform;

    private LoginStates _state;
    private OnlineManager _online;
    private Animator _anim;
    private bool _isSpinning;
    private bool _stopLoginTimer;
    private bool _hasMenuSlidIn;
    private string _GSid;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _online = FindObjectOfType<OnlineManager>();
        _loggedInMenu.SetActive(false);
        _offlineMenu.SetActive(false);
        _facebookMenu.SetActive(false);
        _facebookSpinner.SetActive(false);
        _state = LoginStates.Init;
    }

    void Start()
    {
        EventManager.TriggerEvent(GameSettings.MAIN_MENU_EXISTS);
    }

    void OnEnable()
    {
        Reset();

        EventManager.StartListening(MenuStrings.START_SPINNER, StartFBSpinner);
        EventManager.StartListening(MenuStrings.STOP_SPINNER, StopFBSpinner);

        EventManager.StartListening(OnlineStrings.ONLINE_BUTTON_PRESSED, SetUserPressedLoginState);
        EventManager.StartListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, SetUserOfflineState);
        EventManager.StartListening(OnlineStrings.ONLINE_FALLTHROUGH, SetUserOfflineState);
        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_FACEBOOK, SetUserLoggedInToFacebookState);
        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksState);
    }

    void OnDisable()
    {
        
        EventManager.StopListening(MenuStrings.START_SPINNER, StartFBSpinner);
        EventManager.StopListening(MenuStrings.STOP_SPINNER, StopFBSpinner);

        EventManager.StopListening(OnlineStrings.ONLINE_BUTTON_PRESSED, SetUserPressedLoginState);
        EventManager.StopListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, SetUserOfflineState);
        EventManager.StopListening(OnlineStrings.ONLINE_FALLTHROUGH, SetUserOfflineState);
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_FACEBOOK, SetUserLoggedInToFacebookState);
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksState);
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
                case LoginStates.Online:
                    Online();
                    break;
                case LoginStates.UserPressedLogin:
                    UserPressedLogin();
                    break;
                case LoginStates.UserLoggedInToFacebook:
                    UserLoggedInToFacebook();
                    break;
                case LoginStates.UserLoggedInToGameSparks:
                    UserLoggedInToGameSparks();
                    break;
                case LoginStates.LoginSequenceComplete:
                    LoginSequenceComplete();
                    break;
                case LoginStates.GetPlayerName:
                    GetPlayerName();
                    break;
                default:
                    break;
            }
        }
    }

    private void Init()
    {
        print("MM Init");
        if (!_hasMenuSlidIn)
        {
            print("Run slide animation");
            _anim.SetTrigger(AnimatorStrings.SLIDE_LINES_IN);
            _anim.SetTrigger(AnimatorStrings.SLIDE_SIDE_MENU_IN);
            _hasMenuSlidIn = true;
        }
//
        //PlayerData.instance.SaveUserGSId("");
        if (PlayerData.instance.HasUserLoggedIn)
        {
            _state = LoginStates.Online;
        } 
        else
        {
            _state = LoginStates.Offline;
        }
    }


    private void Idle()
    {
        //nothing
    }

    private void Offline()
    {
        print("MM Offline");

        _facebookSpinner.SetActive(false);
        _offlineMenu.SetActive(true);
        _facebookMenu.SetActive(true);
        _loggedInMenu.SetActive(false);
        PlayerData.instance.HasUserLoggedIn = false;
        EventManager.TriggerEvent(MenuStrings.UPDATE_OFFLINE_MENU);
        _state = LoginStates.Idle;
    }

    private void Online()
    {
        _facebookSpinner.SetActive(false);
        _offlineMenu.SetActive(false);
        _facebookMenu.SetActive(false);
        _loggedInMenu.SetActive(true);
        EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);
        _state = LoginStates.Idle;
    }

    private void UserPressedLogin()
    {
        print("MM UserPressedLogin");
        StartFBSpinner();
        _facebookMenu.SetActive(false);
        StartCoroutine(Utilities.CheckInternetConnection((isConnected) => {
            if(isConnected)
            {
                _online.FacebookLogin();         
            }
            else
            {
                _state = LoginStates.Offline;
            }
        }));


        _state = LoginStates.Idle; 
    }

    private void UserLoggedInToFacebook()
    {
        print("MM UserLoggedInToFacebook");
        _online.GameSparksLogin();
        _state = LoginStates.Idle;
    }

    private void UserLoggedInToGameSparks()
    {
        print("MM UserLoggedInToGamesparks");
        _GSid = PersistentDataManager.LoadGSUserId();
        //Get the data
        StartCoroutine(PersistentDataManager.LoadPlayerData(_GSid, (response) => {
            print("all data should be loaded");
            PlayerData.instance.Scores = response;
            _state = LoginStates.GetPlayerName;
        }));
        _state = LoginStates.Idle;

    }

    private void GetPlayerName()
    {
        print("MM GetPlayerName");
        StartCoroutine(PersistentDataManager.LoadPlayerName(_GSid, (response) => {
            PlayerData.instance.Scores.userName = response.ToString();
            print("GS Username: " + response.ToString());
            StopFBSpinner();
            EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);

            _state = LoginStates.LoginSequenceComplete;
        }));

        _state = LoginStates.Idle;
    }

    private void LoginSequenceComplete()
    {
        print("MM LoginSequenceComplete");
        StopFBSpinner();
        _facebookMenu.SetActive(false);
        _offlineMenu.SetActive(false);
        _loggedInMenu.SetActive(true);
        //FreezeSlideInAnimation();
        PlayerData.instance.HasUserLoggedIn = true;
        EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);

        _state = LoginStates.Idle;
    }


    private void SetUserPressedLoginState()
    {
        _state = LoginStates.UserPressedLogin;
        FreezeSlideInAnimation();
    }

    private void SetUserLoggedInToFacebookState()
    {
        _state = LoginStates.UserLoggedInToFacebook;
    }

    private void SetUserLoggedInToGamesparksState()
    {
        _state = LoginStates.UserLoggedInToGameSparks;
    }

    private void SetUserOfflineState()
    {
        _state = LoginStates.Offline;
    }


    private void StartFBSpinner()
    {   
        if(!_isSpinning)
        {
            _isSpinning = true;
            _facebookSpinner.SetActive(true);
            _anim.SetTrigger(AnimatorStrings.TRIGGER_FB_SPINNER_ON);    
        }
    }

    private void StopFBSpinner()
    {
        if(_isSpinning)
        {
            _isSpinning = false;
            _anim.SetTrigger(AnimatorStrings.TRIGGER_FB_SPINNER_OFF);   
            _facebookSpinner.SetActive(false);
        }
    }

    private void Reset()
    {
        _state = LoginStates.Init;
    }

    private void ResetSlideInAnimation()
    {
        print("resetSlideInAnimation");
        _anim.Play("MenuSlideIn", -1, 0f);
    }

    private void FreezeSlideInAnimation()
    {
        print("freezeSlideInAnimation");
        _anim.Play("MenuSlideIn", -1, 1f);
    }

}

public enum LoginStates
{
    Init,
    Idle,
    Offline,
    Online,
    UserPressedLogin,
    UserLoggedInToFacebook,
    UserLoggedInToGameSparks,
    LoginSequenceComplete,
    GetScores,
    GetPlayerName

}
