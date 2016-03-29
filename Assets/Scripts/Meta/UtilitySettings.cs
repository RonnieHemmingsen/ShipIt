﻿using UnityEngine;
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
    public const string BOLT_TOKEN_USED = "BoltTokenUsed";

    public const string ENGAGE_LUDICROUS_SPEED = "LudicrousSpeedOn";
    public const string DISENGAGE_LUDICROUS_SPEED = "LudicrousSpeedOff";
    public const string INVULNERABILITY_ON = "InvulnerabilityOn";
    public const string INVULNERABILITY_OFF = "InvulnerabilityOff";
    public const string GET_REKT = "GetRekt";

    public const string START_CAMERA_SHAKE = "StartCameraShake";
    public const string STOP_CAMERA_SHAKE = "StopCameraShake";
    public const string START_BULLETENEMY_SHOOTING = "StartBulletEnemyShooting";
    public const string STOP_BULLETENEMY_SHOOTING = "StopBulletEnemyShooting";

    public const string ENABLE_GAMEOVER_MENU = "EnableGameoverMenu";
    public const string DISABLE_GAMEOVER_MENU = "DisableGameoverMenu";

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
    public const string TRIGGER_FB_SPINNER_ON = "StartFBSpin";
    public const string TRIGGER_FB_SPINNER_OFF = "StopFBSpin";
}
   
public static class GameSettings
{
    public const float CROSSFADE_ALPHA_VALUE = 0.2f;

    public const string GAME_SCENE = "Game";
    public const string MAIN_MENU_SCENE = "Start Menu";
    public const string BOOT_SCENE = "BootScene";
    public const string URL_CHECK = "http://google.com";
    public const string ONLINE_BUTTON_PRESSED = "OnlineButtonPressed";
    public const string LOGGED_IN_TO_GAMESPARKS = "LoggedInToGamesparks";
    public const string FACEBOOK_INIT_DONE = "FacebookInitDone";

    public const string HAS_DESTROY_ALL_TOKEN_HELP_BEEN_DISPLAYED = "HasDestroyAllTokenHelpBeenDisplayed";
    public const string HAS_SHIELD_TOKEN_HELP_BEEN_DISPLAYED = "HasShieldTokenHelpBeenDisplayed";
    public const string HAS_SPEED_TOKEN_HELP_BEEN_DISPLAYED = "HasSpeedTokenHelpBeenDisplayed";

    public const string START_SPINNER = "StartSpinner";
    public const string STOP_SPINNER = "StopSpinner";
    public const string UPDATE_LOGGED_IN_MENU = "UpdateLoggedInMenu";


    public const string PAUSE_GAME = "PauseGame";
    public const string START_GAME = "StartGame";
    public const string UPDATE_ACTIVE_SCENE = "BootGame";
    public const string GAME_OVER = "GameOver";
    public const string GAME_HAS_STARTED = "GameHasStarted";

    public const string SAVE_PATH = "/thefutureinfo.dat";
    public const string SAVE_DATA = "SaveData";
    public const string LOAD_DATA = "LoadData";


    public const int SMALL_COIN_VALUE = 1;
    public const int BIG_COIN_VALUE = 50;

    public const int COST_OF_DEATH = 5;
}

public static class BackendVariables
{
    public const string HIGHSCORE_BOARD = "HIGH_SCORE_LB";
    
    public const string TRAVELSCORE_ATTRIBUTE = "TRAVEL_ATTR";
    public const string COINSCORE_ATTRIBUTE = "SCORE_ATTR";

    public const string PLAYER_DATA = "playerState";
    public const string PLAYER_DATA_TIMESTAMP = "TimeStamp";
    public const string PLAYER_DATA_COINS = "Coins";
    public const string PLAYER_DATA_LAST_TRAVEL = "LastTravelDist";
    public const string PLAYER_DATA_MAX_TRAVEL = "MaxTravelDist";
}

public static class TextStrings
{
    public const string DESTROY_ALL_AQUIRED = "Destroy All The Things!";
    public const string INVULNERABILITY_AQUIRED = "Shield!";
    public const string SPEED_AQUIRED = "Ludicrous Speed!";
    public const string BOLT_AQUIRED = "+1 Bolt!";
    public const string COIN_AQUIRED = "+1";
}
