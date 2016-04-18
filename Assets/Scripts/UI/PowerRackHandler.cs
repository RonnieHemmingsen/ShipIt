using UnityEngine;
using System.Collections;

public class PowerRackHandler : MonoBehaviour {

    private CanvasGroup _powerRack;
	// Use this for initialization
	void Awake () {
	
        _powerRack = GetComponent<CanvasGroup>();
	}
	
    void OnEnable()
    {
        EventManager.StartListening(MenuStrings.ENABLE_DESTROYALL_INSTRUCTIONS, DisablePowerRack);
        EventManager.StartListening(MenuStrings.ENABLE_SHIELD_INSTRUCTIONS, DisablePowerRack);
        EventManager.StartListening(MenuStrings.ENABLE_SPEED_INSTRUCTIONS, DisablePowerRack);
        EventManager.StartListening(MenuStrings.ENABLE_PAUSE_MENU, DisablePowerRack);
        EventManager.StartListening(MenuStrings.ENABLE_GAMEOVER_MENU, DisablePowerRack);

        EventManager.StartListening(MenuStrings.DISABLE_PAUSE_MENU, EnablePowerRack);
        EventManager.StartListening(MenuStrings.DISABLE_GAMEOVER_MENU, EnablePowerRack);
        EventManager.StartListening(MenuStrings.DISABLE_INSTRUCTIONS_SCREEN, EnablePowerRack);
    }

    void OnDisable()
    {
        EventManager.StopListening(MenuStrings.ENABLE_DESTROYALL_INSTRUCTIONS, DisablePowerRack);
        EventManager.StopListening(MenuStrings.ENABLE_SHIELD_INSTRUCTIONS, DisablePowerRack);
        EventManager.StopListening(MenuStrings.ENABLE_SPEED_INSTRUCTIONS, DisablePowerRack);
        EventManager.StopListening(MenuStrings.ENABLE_PAUSE_MENU, DisablePowerRack);
        EventManager.StopListening(MenuStrings.ENABLE_GAMEOVER_MENU, DisablePowerRack);

        EventManager.StopListening(MenuStrings.DISABLE_PAUSE_MENU, EnablePowerRack);
        EventManager.StopListening(MenuStrings.DISABLE_GAMEOVER_MENU, EnablePowerRack);
        EventManager.StopListening(MenuStrings.DISABLE_INSTRUCTIONS_SCREEN, EnablePowerRack);
    }

    private void DisablePowerRack()
    {
        _powerRack.blocksRaycasts = false;
    }

    private void EnablePowerRack()
    {
        _powerRack.blocksRaycasts = true;
    }
}

