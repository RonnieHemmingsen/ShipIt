using UnityEngine;
using System.Collections;

public class InstructionsScreenMenuHandler : MonoBehaviour {

    [SerializeField]
    private CanvasGroup _instructionsScreenMenu;
    [SerializeField]
    private CanvasGroup _destroyAllInstructions;
    [SerializeField]
    private CanvasGroup _shieldInstructions;
    [SerializeField]
    private CanvasGroup _speedInstructions;
    [SerializeField]
    private CanvasGroup _powerRack;

	// Use this for initialization
	void Start () {
        Utilities.MenuOff(_instructionsScreenMenu);
        Utilities.MenuOff(_destroyAllInstructions);
        Utilities.MenuOff(_shieldInstructions);
        Utilities.MenuOff(_speedInstructions);
	}
	
    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.ENABLE_DESTROYALL_INSTRUCTIONS, EnableDestroyAllInstructions);
        EventManager.StartListening(MenuStrings.ENABLE_SHIELD_INSTRUCTIONS, EnableShieldInstructions);
        EventManager.StartListening(MenuStrings.ENABLE_SPEED_INSTRUCTIONS, EnableSpeedInstructions);
        EventManager.StartListening(MenuStrings.DISABLE_INSTRUCTIONS_SCREEN, DisableInstructionsScreen);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.ENABLE_DESTROYALL_INSTRUCTIONS, EnableDestroyAllInstructions);
        EventManager.StopListening(MenuStrings.ENABLE_SHIELD_INSTRUCTIONS, EnableShieldInstructions);
        EventManager.StopListening(MenuStrings.ENABLE_SPEED_INSTRUCTIONS, EnableSpeedInstructions);
        EventManager.StopListening(MenuStrings.DISABLE_INSTRUCTIONS_SCREEN, DisableInstructionsScreen);
    }

    private void EnableDestroyAllInstructions()
    {
        Utilities.Pause();
        Utilities.MenuOn(_instructionsScreenMenu);
        Utilities.MenuOn(_destroyAllInstructions);
    }

    private void EnableShieldInstructions()
    {
        Utilities.Pause();
        Utilities.MenuOn(_instructionsScreenMenu);
        Utilities.MenuOn(_shieldInstructions);
    }

    private void EnableSpeedInstructions()
    {
        Utilities.Pause();
        Utilities.MenuOn(_instructionsScreenMenu);
        Utilities.MenuOn(_speedInstructions);
    }

    private void DisableInstructionsScreen()
    {
        Utilities.UnPause();
        Utilities.MenuOff(_instructionsScreenMenu);
        Utilities.MenuOff(_destroyAllInstructions);
        Utilities.MenuOff(_shieldInstructions);
        Utilities.MenuOff(_speedInstructions);

    }


}
