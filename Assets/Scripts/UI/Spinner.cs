using UnityEngine;
using System.Collections;

public class Spinner : MonoBehaviour {

    [SerializeField]
    private MainMenuHandler _mainMenu;
    [SerializeField]
    private GameObject _innerSpinner;
    [SerializeField]
    private GameObject _outerSpinner;

    private CanvasGroup _spinnerMenu;
    private bool _isSpinning;

    void Awake()
    {
        _spinnerMenu = GetComponent<CanvasGroup>();
        Utilities.MenuOff(_spinnerMenu);
    }

    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.START_SPINNER, StartSpinner);
        EventManager.StartListening(MenuStrings.STOP_SPINNER, StopSpinner);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.START_SPINNER, StartSpinner);
        EventManager.StopListening(MenuStrings.STOP_SPINNER, StopSpinner);
    }

    private void StartSpinner()
    {   
        if(!_isSpinning)
        {
            print("Spinner On");
            _isSpinning = true;
            Utilities.MenuOn(_spinnerMenu);
            _mainMenu.Anim.SetTrigger(AnimatorStrings.TRIGGER_SPINNER_ON);    
        }
    }

    private void StopSpinner()
    {
        if(_isSpinning)
        {
            print("Spinner Off");
            _isSpinning = false;
            _mainMenu.Anim.SetTrigger(AnimatorStrings.TRIGGER_SPINNER_OFF);   
            Utilities.MenuOff(_spinnerMenu);
        }
    }


}
