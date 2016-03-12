﻿using UnityEngine;
using System.Collections;

public static class EventStrings
{
    public const string PLAYER_DEAD = "PlayerDead";
    public const string HAZARD_KILL = "HazardKill";

    public const string DISPLAY_TUTORIAL = "DisplayTutorial";

    public const string ENEMY_DESTROYED = "EnemyDestroyed";
    public const string SPEED_INCREASE = "SpeedIncrease";
    public const string TOGGLE_LASER_ENEMY_SPAWNING = "ToggleLaserEnemySpawning";
    public const string TOGGLE_BULLET_ENEMY_SPAWNING = "ToggleBulletEnemySpawning";
    public const string TOGGLE_ASTEROID_SPAWNING = "ToggleAsteroidSpawning";

    public const string GRAB_COIN = "GrabCoin";
    public const string GRAB_BIG_COIN = "GrabBigCoin"; 
    public const string GRAB_LUDICROUS_SPEED_TOKEN = "GrabLudicrousSpeedToken";
    public const string GRAB_INVUNERABILITY_TOKEN = "GrabInvulnerabilityToken";
    public const string GRAB_DESTROY_ALL_TOKEN = "GrabDestroyAllToken";

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

    public const string USER_LOGGED_IN = "UserLoggedIn";
}

//fairly important to keep consistent with tag manager!!
public static class TagStrings
{
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
}
   
public static class GameSettings
{
    public const float CROSSFADE_ALPHA_VALUE = 0.2f;

    public const string HAS_DESTROY_ALL_TOKEN_HELP_BEEN_DISPLAYED = "HasDestroyAllTokenHelpBeenDisplayed";
    public const string HAS_SHIELD_TOKEN_HELP_BEEN_DISPLAYED = "HasShieldTokenHelpBeenDisplayed";
    public const string HAS_SPEED_TOKEN_HELP_BEEN_DISPLAYED = "HasSpeedTokenHelpBeenDisplayed";
    public const string LOAD_PROGRESS_TEXT = "Loading: ";
    public const string LOAD_LEVEL_GAME = "Game";
    public const string LOAD_LEVEL_MENU = "Start Menu";
    public const string GAME_STARTED = "Game Started";

    public const string COIN_SCORE = "CoinScore";
    public const string TRAVEL_SCORE = "TravelScore";
}
