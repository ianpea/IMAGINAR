using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleARCore;

[System.Serializable]
public class GameModel
{
    public static float imaginationSpawnCooldown = 1.0f;
    public static float imaginationLastSpawnTime = 0.0f;
    public static float enemySpawnCooldown = 6.0f;
    public static float enemyLastSpawnTime = -enemySpawnCooldown;
    public static float phenomenonSpawnCooldown = 9.0f;
    public static float phenomenonLastSpawnTime = -phenomenonSpawnCooldown;
    public static float bossSpawnCooldown = 75.0f;
    public static float bossLastSpawnTime = -bossSpawnCooldown;
    public static float currentTime;
    public GAME_STATE currentState = GAME_STATE.Player;
    public Anchor sessionAnchor;

    // boss stuff
    public GameObject bossPrefab;
    public float bossSpawnTriggerIP = 800.0f;
    public float bossSpawnDistance = 65.0f;

    public bool isSaving;

    /// <summary>
    /// Maximum distance (Meters) a unit can spawn from player.
    /// </summary>
    public const float MAX_SPAWN_DISTANCE = 5.0f;
}

public enum GAME_STATE
{
    Player = 0,
    Pet = 1
}