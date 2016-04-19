using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour {
    [SerializeField]
    private CanvasGroup _topLevelTopMenu;
    [SerializeField]
    private CanvasGroup _topLevelSideMenu;
    [SerializeField]
    private CanvasGroup _topLevelLinesMenu;
    [SerializeField]
    private GameObject _title;
    [SerializeField]
    private CanvasGroup _facebookMenu;
    [SerializeField]
    private CanvasGroup _offlineMenu;
    [SerializeField]
    private CanvasGroup _onlineMenu;
    [SerializeField]
    private Vector3 _sideMenuDefaultPositon;
    [SerializeField]
    private Vector3 _linesDefaultPosition;

    private CanvasGroup _mainMenu;
    private LoginStates _state;
    private OnlineManager _online;
    private Animator _anim;
    private bool _isSpinning;
    private bool _hasMenuSlidIn;
    private bool _wasFilthyFacebookHack;
    private string _GSid;
    private string _currentActiveTopLevelMenu;

    public Animator Anim
    {
        get { return _anim; }
        set { _anim = value; }
    }

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _online = FindObjectOfType<OnlineManager>();
        _mainMenu = GetComponent<CanvasGroup>();

        Utilities.MenuOff(_onlineMenu);
        Utilities.MenuOff(_offlineMenu);
        Utilities.MenuOff(_facebookMenu);
        _state = LoginStates.Init;
    }

    void Start()
    {
        _currentActiveTopLevelMenu = _topLevelSideMenu.name;
        EventManager.TriggerEvent(GameSettings.MAIN_MENU_EXISTS);
    }

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, Reset);

        EventManager.StartListening(OnlineStrings.ONLINE_BUTTON_PRESSED, SetUserPressedLoginState);
        EventManager.StartListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, SetUserOfflineState);
        EventManager.StartListening(OnlineStrings.ONLINE_FALLTHROUGH, SetUserOfflineState);
        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_FACEBOOK, SetUserLoggedInToFacebookState);
        EventManager.StartListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksState);

        EventManager.StartListening(MenuStrings.LEADER_BOARD_MENU_PRESSED, LeaderBoardMenuPressed);
        EventManager.StartListening(MenuStrings.PROFILE_MENU_PRESSED, ProfileMenuPressed);
        EventManager.StartListening(MenuStrings.CREDITS_PRESSED, CreditsPressed);
        EventManager.StartListening(MenuStrings.CLEAR_MENUS_FOR_GAME, ClearMenusForGamePressed);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, Reset);

        EventManager.StopListening(OnlineStrings.ONLINE_BUTTON_PRESSED, SetUserPressedLoginState);
        EventManager.StopListening(OnlineStrings.OFFLINE_BUTTON_PRESSED, SetUserOfflineState);
        EventManager.StopListening(OnlineStrings.ONLINE_FALLTHROUGH, SetUserOfflineState);
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_FACEBOOK, SetUserLoggedInToFacebookState);
        EventManager.StopListening(OnlineStrings.LOGGED_IN_TO_GAMESPARKS, SetUserLoggedInToGamesparksState);

        EventManager.StopListening(MenuStrings.LEADER_BOARD_MENU_PRESSED, LeaderBoardMenuPressed);
        EventManager.StopListening(MenuStrings.PROFILE_MENU_PRESSED, ProfileMenuPressed);
        EventManager.StartListening(MenuStrings.CREDITS_PRESSED, CreditsPressed);
        EventManager.StopListening(MenuStrings.CLEAR_MENUS_FOR_GAME, ClearMenusForGamePressed);
    }

    #region login states

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

        Utilities.MenuOff(_onlineMenu);
        Utilities.MenuOn(_offlineMenu);
        Utilities.MenuOn(_facebookMenu);

        _online.OnlineLogout();
        PlayerData.instance.HasUserLoggedIn = false;
        EventManager.TriggerEvent(MenuStrings.UPDATE_OFFLINE_MENU);
        _state = LoginStates.Idle;
    }

    private void Online()
    {
        print("MM Online");
        Utilities.MenuOff(_offlineMenu);
        Utilities.MenuOff(_facebookMenu);
        Utilities.MenuOn(_onlineMenu);

        EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);
        EventManager.TriggerEvent(MenuStrings.UPDATE_LEADERBOARDS);
        _state = LoginStates.Idle;
    }

    private void UserPressedLogin()
    {
        print("MM UserPressedLogin");
        EventManager.TriggerEvent(MenuStrings.START_SPINNER);

        _wasFilthyFacebookHack = true;

        Utilities.MenuOff(_facebookMenu);
        Utilities.MenuOff(_topLevelSideMenu);
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
            EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);

            _state = LoginStates.LoginSequenceComplete;
        }));

        _state = LoginStates.Idle;
    }

    private void LoginSequenceComplete()
    {
        print("MM LoginSequenceComplete");

        PlayerData.instance.HasUserLoggedIn = true;
        EventManager.TriggerEvent(MenuStrings.UPDATE_ONLINE_MENU);
        EventManager.TriggerEvent(MenuStrings.UPDATE_LEADERBOARDS);
        EventManager.TriggerEvent(MenuStrings.STOP_SPINNER);

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


   

    private void Reset()
    {
        Utilities.MenuOn(_mainMenu);
        _currentActiveTopLevelMenu = _topLevelSideMenu.name;
        _hasMenuSlidIn = false;
        _state = LoginStates.Init;
    }

    //hack to avoid facebook menu snafu
    private void StopMenuFromSlidingIn()
    {
        print("StopMenuFromSlidingIn");
        _anim.Play("MenuSlideIn", -1, 0f);
    }

    private void FreezeSlideInAnimation()
    {
        print("freezeSlideInAnimation");
        _anim.Play("MenuSlideIn", -1, 1f);

    }

    #endregion

    #region menu buttons pressed
    private void LeaderBoardMenuPressed()
    {
        //Already on the top menu view. do nothing
        if(_currentActiveTopLevelMenu == MenuStrings.TOP_LEVEL_TOP_MENU)
        {
            return;
        }

        //Switch from side menu to top menu
        if(_currentActiveTopLevelMenu == MenuStrings.TOP_LEVEL_SIDE_MENU)
        {
            _currentActiveTopLevelMenu = MenuStrings.TOP_LEVEL_TOP_MENU;
            StartCoroutine(SwitchTopLevelMenus(
                AnimatorStrings.SLIDE_SIDE_MENU_OUT, 
                AnimatorStrings.SLIDE_TOP_MENU_IN));
        }
    }

    private void ProfileMenuPressed()
    {
        //Already on the side menu view. do nothing
        if(_currentActiveTopLevelMenu == MenuStrings.TOP_LEVEL_SIDE_MENU)
        {
            return;
        }

        //Switch from top menu to side menu
        if(_currentActiveTopLevelMenu == MenuStrings.TOP_LEVEL_TOP_MENU)
        {
            

            _currentActiveTopLevelMenu = MenuStrings.TOP_LEVEL_SIDE_MENU;
            StartCoroutine(SwitchTopLevelMenus(
                AnimatorStrings.SLIDE_TOP_MENU_OUT, 
                AnimatorStrings.SLIDE_SIDE_MENU_IN));
        }
    }

    private void CreditsPressed()
    {
        //TODO: Refactor to make sense
        ProfileMenuPressed();
    }

    private void ClearMenusForGamePressed()
    {
        StartCoroutine(ClearForGame());
    }


    private IEnumerator ClearForGame()
    {
        //Check which of the toplevel menus are active. then slide it out
        if(_currentActiveTopLevelMenu == MenuStrings.TOP_LEVEL_SIDE_MENU)
        {
            _anim.SetTrigger(AnimatorStrings.SLIDE_SIDE_MENU_OUT);    
        }
        else
        {
            _anim.SetTrigger(AnimatorStrings.SLIDE_TOP_MENU_OUT);
        }

        //slide the line menus out
        _anim.SetTrigger(AnimatorStrings.SLIDE_LINES_OUT);

        yield return new WaitForSeconds(1);

        //make sure all buttons are switched off
        Utilities.MenuOff(_mainMenu);
        //Start new game
        EventManager.TriggerEvent(GameSettings.START_GAME);
    }

    private IEnumerator SwitchTopLevelMenus(string menuOut, string menuIn)
    {
        _anim.SetTrigger(menuOut);

        yield return new WaitForSeconds(0.7f);

        if(_wasFilthyFacebookHack)
        {
            StopMenuFromSlidingIn();
            Utilities.MenuOn(_topLevelSideMenu); //just in case
            _wasFilthyFacebookHack = false;
        }
        _anim.SetTrigger(menuIn);
    }
    #endregion
        
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
