using UnityEngine;
using System.Collections;

public static class EventStrings
{
    public const string PLAYER_DEAD = "PlayerDead";
    public const string HAZARD_KILL = "HazardKill";

    public const string DISPLAY_TUTORIAL = "DisplayTutorial";

    public const string ENEMY_DESTROYED = "EnemyDestroyed";
    public const string SPEED_INCREASE = "SpeedIncrease";
    public const string SPEED_DECREASE = "SpeedDecrease";
    public const string TOGGLE_LASER_ENEMY_SPAWNING = "ToggleLaserEnemySpawning";
    public const string TOGGLE_BULLET_ENEMY_SPAWNING = "ToggleBulletEnemySpawning";
    public const string TOGGLE_ASTEROID_SPAWNING = "ToggleAsteroidSpawning";

    public const string GRAB_COIN = "GrabCoin";
    public const string GRAB_BIG_COIN = "GrabBigCoin"; 
    public const string GRAB_LUDICROUS_SPEED_TOKEN = "GrabLudicrousSpeedToken";
    public const string GRAB_INVUNERABILITY_TOKEN = "GrabInvulnerabilityToken";
    public const string GRAB_DESTROY_ALL_TOKEN = "GrabDestroyAllToken";
    public const string GRAB_BOLT_TOKEN = "GrabBoltToken";
    public const string PLAYER_SHOOTS = "PlayerShoots";

    public const string ENGAGE_LUDICROUS_SPEED = "LudicrousSpeedOn";
    public const string DISENGAGE_LUDICROUS_SPEED = "LudicrousSpeedOff";
    public const string INVULNERABILITY_ON = "InvulnerabilityOn";
    public const string INVULNERABILITY_OFF = "InvulnerabilityOff";
    public const string GET_REKT = "GetRekt";

    public const string START_CAMERA_SHAKE = "StartCameraShake";
    public const string STOP_CAMERA_SHAKE = "StopCameraShake";
    public const string START_BULLETENEMY_SHOOTING = "StartBulletEnemyShooting";
    public const string STOP_BULLETENEMY_SHOOTING = "StopBulletEnemyShooting";

    public const string REMOVE_FROM_ALIVE_LIST = "RemoveFromAliveList";

    public const string HAZARD_OUT_OF_BOUNDS = "HazardOutOfBounds";
    public const string ENEMY_OUT_OF_BOUNDS = "EnemyOutOfBounds";
    public const string TOKEN_OUT_OF_BOUNDS = "TokenOutOfBounds";

    public const string TEXT_TWEEN = "TextTween";

    public const string ADD_TO_GLOBAL_COINSCORE = "AddToGlobalCoinScore";
    public const string SUBTRACT_FROM_GLOBAL_COINSCORE = "SubtractFromGlobalCoinScore";

    public const string USER_LOGGED_IN = "UserLoggedIn";
    public const string GET_GAME_MANAGER = "GetGameManager";
}

//fairly important to keep consistent with tag manager!!
public static class ObjectStrings
{
    //These are tags
    public const string HAZARD = "Hazard";
    public const string PLAYER = "Player";
    public const string LASER_ENEMY = "LaserEnemy";
    public const string BULLET_ENEMY = "BulletEnemy";
    public const string INVULNERABLE = "Invulnerable";
    public const string DESTROY_ALL = "DestroyAll";
    public const string COIN = "Coin";
    public const string BOLT = "Bolt";
    public const string ENEMY_BOLT = "EnemyBolt";
    public const string BULLET = "Bullet";
    public const string BIG_COIN = "BigCoin";
    public const string LUDICROUS_SPEED = "LudicrousSpeed";
    public const string BOLT_TOKEN = "BoltToken";

    //These are not tags
    public const string TWEEN_TEXT_OUT = "TweenTextOut";


}

public static class AnimatorStrings
{
    public const string TRIGGER_SPINNER_ON = "StartSpin";
    public const string TRIGGER_SPINNER_OFF = "StopSpin";

    public const string SLIDE_LINES_IN = "SlideLinesIn";
    public const string SLIDE_LINES_OUT = "SlideLinesOut";
    public const string SLIDE_SIDE_MENU_IN = "SlideSideMenuIn";
    public const string SLIDE_SIDE_MENU_OUT = "SlideSideMenuOut";
    public const string SLIDE_TOP_MENU_IN = "SlideTopMenuIn";
    public const string SLIDE_TOP_MENU_OUT = "SlideTopMenuOut";
}
   
public static class GameSettings
{
    public const float CROSSFADE_ALPHA_VALUE = 0.2f;

    public const string GAME_SCENE = "Game";
    public const string MAIN_MENU_SCENE = "Start Menu";
    public const string BOOT_SCENE = "BootScene";

    public const string HAS_DESTROY_ALL_TOKEN_HELP_BEEN_DISPLAYED = "HasDestroyAllTokenHelpBeenDisplayed";
    public const string HAS_SHIELD_TOKEN_HELP_BEEN_DISPLAYED = "HasShieldTokenHelpBeenDisplayed";
    public const string HAS_SPEED_TOKEN_HELP_BEEN_DISPLAYED = "HasSpeedTokenHelpBeenDisplayed";

    public const string PAUSE_GAME = "PauseGame";
    public const string START_GAME = "StartGame";
    public const string RESET_GAME = "ResetGame";
    public const string BOOT_GAME = "BootGame";
    public const string GAME_OVER = "GameOver";
    public const string MURDER_PLAYER = "MurderPlayer"; //for quitting manually
    public const string GAME_HAS_STARTED = "GameHasStarted";
    public const string MAIN_MENU_EXISTS = "MainMenuExists";
    public const string GAME_UI_EXISTS = "GameUIExists";

    public const string SAVE_PATH = "/thefutureinfo.dat";
    public const string SAVE_DATA = "SaveData";
    public const string LOAD_DATA = "LoadData";


    public const int SMALL_COIN_VALUE = 1;
    public const int BIG_COIN_VALUE = 50;

    public const int COST_OF_DEATH = 10;
}

public static class BackendVariables
{
    public const string HIGHSCORE_BOARD = "HIGH_SCORE_LB";
    
    public const string TRAVELSCORE_ATTRIBUTE = "TRAVEL_ATTR";
    public const string COINSCORE_ATTRIBUTE = "SCORE_ATTR";

    public const string PLAYER_DATA = "playerState";
    public const string PLAYER_DATA_TIMESTAMP = "TimeStamp";
    public const string PLAYER_DATA_TOTAL_COINS = "TotalCoins";
    public const string PLAYER_DATA_LAST_COINS = "LastTripCoins";
    public const string PLAYER_DATA_LAST_TRAVEL = "LastTravelDist";
    public const string PLAYER_DATA_MAX_TRAVEL = "MaxTravelDist";
}

public static class TextStrings
{
    //Tween texts
    public const string DESTROY_ALL_AQUIRED = "Destroy All The Things!";
    public const string INVULNERABILITY_AQUIRED = "Shield!";
    public const string SPEED_AQUIRED = "Ludicrous Speed!";
    public const string BOLT_AQUIRED = "+1 Bolt!";
    public const string COIN_AQUIRED = "+1";

    //Scores
    public const string FUTURE_GREENS_SCORE = "Future Greens";
    public const string LIGHTYEARS_TRAVELLED = "Lightyears Travelled";
    public const string FG_LAST = "Greens Collected on the last trip: ";
    public const string FG_TOTAL = "Greens collected in total: ";
    public const string LAST_TRIP_LENGTH = "Last Trip: ";
    public const string BEST_TRIP_LENGTH = "Best Trip: ";

    public const string TOTAL = "Total: ";
        
}

public static class MenuStrings
{
    public const string START_SPINNER = "StartSpinner";
    public const string STOP_SPINNER = "StopSpinner";

    public const string UPDATE_ONLINE_MENU = "UpdateLoggedInMenu";
    public const string UPDATE_OFFLINE_MENU = "UpdateOfflineMenu";

    public const string UPDATE_LEADERBOARDS = "UpdateLeaderboards";

    public const string TOP_LEVEL_SIDE_MENU = "TopLevelSideMenu";
    public const string TOP_LEVEL_TOP_MENU = "TopLevelTopMenu";

    public const string LEADER_BOARD_MENU_PRESSED = "LeaderBoardMenuPressed";
    public const string PROFILE_MENU_PRESSED = "ProfileMenuPressed";
    public const string CREDITS_PRESSED = "CreditsPressed";
    public const string CLEAR_MENUS_FOR_GAME = "ClearMenusForGame";

    public const string PAUSE_INTRO_ENABLED = "PauseIntroEnabled";
    public const string PAUSE_INTRO_DISABLED = "PauseIntroDisabled";
    public const string EXIT_TO_MENU = "ExitToMenu";

    public const string ENABLE_GAMEOVER_MENU = "EnableGameoverMenu";
    public const string DISABLE_GAMEOVER_MENU = "DisableGameoverMenu";
    public const string ENABLE_PAUSE_MENU = "EnablePauseMenu";
    public const string DISABLE_PAUSE_MENU = "DisablePauseMenu";

    public const string ENABLE_DESTROYALL_INSTRUCTIONS = "EnableDestroyAllInstructions";
    public const string ENABLE_SHIELD_INSTRUCTIONS = "EnableShieldInstructions";
    public const string ENABLE_SPEED_INSTRUCTIONS = "EnableSpeedInstructions";
    public const string DISABLE_INSTRUCTIONS_SCREEN = "DisableInstructionsScreen";

    public const string ENABLE_INTRO_SCREEN = "EnableIntroScreen";
    public const string DISABLE_INTRO_SCREEN = "DisableIntroScreen";

}

public static class OnlineStrings
{
    public const string URL_CHECK = "http://google.com";
    public const string ONLINE_BUTTON_PRESSED = "OnlineButtonPressed";
    public const string OFFLINE_BUTTON_PRESSED = "OfflineButtonPressed";

    public const string LOGGED_IN_TO_GAMESPARKS = "LoggedInToGamesparks";
    public const string LOGGED_IN_TO_FACEBOOK = "LoggedInToFacebook";
    public const string FACEBOOK_NEW_USER = "FacebookNewUser";
    public const string FACEBOOK_INIT_DONE = "FacebookInitDone";
    public const string ONLINE_FALLTHROUGH = "OnlineFallthrough";

    public const string GS_USER_ID = "GSUserId";
}
