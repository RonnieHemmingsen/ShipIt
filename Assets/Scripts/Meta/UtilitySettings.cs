using UnityEngine;
using System.Collections;

public static class EventStrings
{
    public const string PLAYER_DEAD = "PlayerDead";
    public const string HAZARD_KILL = "HazardKill";

    public const string ENEMY_DESTROYED = "EnemyDestroyed";
    public const string SPEED_INCREASE = "SpeedIncrease";
    public const string TOGGLE_ENEMY_SPAWNING = "ToggleEnemySpawning";
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
}
