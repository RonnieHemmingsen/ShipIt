using UnityEngine;
using System.Collections;

public class GamePausedHandler : MonoBehaviour {

    [SerializeField]
    private bool _pauseGame;

    void OnEnable()
    {
        EventManager.StartListening(GameSettings.GAME_OVER, HandlePause);
    }

    void OnDisable()
    {
        EventManager.StopListening(GameSettings.GAME_OVER, HandlePause);
    }

    private void HandlePause()
    {
        if(_pauseGame)
            Utilities.Pause();
        else
            Utilities.UnPause();
    }
}
