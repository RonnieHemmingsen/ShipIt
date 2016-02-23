using UnityEngine;
using System.Collections;

public static class EventStrings
{
    public const string PLAYERDEAD = "PlayerDead";
    public const string HAZARDKILL = "HazardKill";
    public const string COINGRAB = "CoinGrab";
    public const string ENEMYDESTROYED = "EnemyDestroyed";
    public const string INVULNERABILITYON = "InvulnerabilityOn";
    public const string INVULNERABILITYOFF = "InvulnerabilityOff";
}

//fairly important to keep consistent with tag manager!!
public static class TagStrings
{
    public const string HAZARD = "Hazard";
    public const string PLAYER = "Player";
    public const string ENEMY = "Enemy";
    public const string INVULNERABLE = "Invulnerable";
    public const string DESTROYALL = "DestroyAll";
    public const string COIN = "Coin";
    public const string BOLT = "Bolt";
    public const string ENEMYBOLT = "EnemyBolt";
}
   
public static class GameSettings
{
    public const float CROSSFADE_APHA_VALUE = 0.2f;
}
