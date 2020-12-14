using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleARCore;

public class GameController
{
    public delegate void OnTransform();
    public OnTransform onTransform;

    GameManager V;
    GameModel M;
    ScoreManager SM;
    /// <summary>
    /// MVC setup.
    /// </summary>
    /// 
    public delegate void OnSave();
    public OnSave onSave;

    public void GameInit()
    {
        V = GameManager.Instance;
        M = V.Model;

        SM = ScoreManager.Instance;

        M.currentState = GAME_STATE.Player;

        LoadGame();
    }

    /// <summary>
    /// Sets an anchor for the game after the game starts.
    /// </summary>
    public void InitAnchor()
    {
        Pose tempPose = new Pose(new Vector3(0, 0, 0), Quaternion.identity);

        var anchor = Session.CreateAnchor(tempPose);
        M.sessionAnchor = anchor;
    }

    // Start is called before the first frame update
    public void Start()
    {
        GameInit();
    }

    // Update is called once per frame
    public void Update()
    {
        if(GameManager.Instance.Model.currentState == GAME_STATE.Player)
        {
            RandomAnimal(GameModel.imaginationSpawnCooldown);
            RandomEnemy(GameModel.enemySpawnCooldown);
            RandomPhenomenon(GameModel.phenomenonSpawnCooldown);
        }
        else
        { 
            RandomEnemy(GameModel.enemySpawnCooldown);
            RandomPhenomenon(GameModel.phenomenonSpawnCooldown);
        }

        SpawnBoss(GameModel.bossSpawnCooldown);

        if (M.sessionAnchor == null)
        {
            InitAnchor();
        }
    }

    /// <summary>
    /// Switches game states and responsible for executing transition activity.
    /// </summary>
    public void SwitchState()
    {
        if(M.currentState == GAME_STATE.Player)
        {
            M.currentState = GAME_STATE.Pet;
            TransitionToPet();

        }
        else if(M.currentState == GAME_STATE.Pet)
        {
            M.currentState = GAME_STATE.Player;
            TransitionToPlayer();
        }
    }

    /// <summary>
    /// Do things here when transition to pet state.
    /// </summary>
    public void TransitionToPet()
    {
        /// Set all active imaginations (phenomenons and animals) to inactive.
        foreach (Phenomenon p in PhenomenonPool.Instance.pool)
        {
            if(p.spawned)
                p.gameObject.SetActive(false);
        }
        foreach (Animal a in AnimalPool.Instance.pool)
        {
            if (a.spawned)
                a.gameObject.SetActive(false);
        }
        /// Cancel fade image coroutine if its running, and fade back to alpha = 0.0f;
        if (UIManager.Instance.fadeCo != null)
        {
            V.StopCoroutine(UIManager.Instance.fadeCo);
        }
        Fade.isFading = false;
        UIManager.Instance.fadeCo = V.StartCoroutine(Fade.FadeTo(0.0f, 1.5f, UIManager.Instance.fadeImg));

        //camera effects and ui delegate

        /// Monster wander once player enter pet state.
        foreach (Monster m in MonsterPool.Instance.pool)
        {
            m.isWander = true;
        }
    }

    /// <summary>
    /// Do things here when transition to player state.
    /// </summary>
    public void TransitionToPlayer()
    {
        /// Set all inactive imaginations (phenomenons and animals) to active.
        foreach (Phenomenon p in PhenomenonPool.Instance.pool)
        {
            if (p.spawned)
                p.gameObject.SetActive(true);
        }
        foreach (Animal a in AnimalPool.Instance.pool)
        {
            if (a.spawned)
                a.gameObject.SetActive(true);
        }
        // todo camera effects and ui delegate
        /// Stop onster wander once player enter pet state.
        foreach (Monster m in MonsterPool.Instance.pool)
        {
            m.isWander = false;
        }
    }

    /// <summary>
    /// Fade image in front of player to black.
    /// </summary>
    public void FadeToBlack()
    {
        UIManager.Instance.fadeCo = V.StartCoroutine(Fade.FadeTo(1.0f, 5.0f, UIManager.Instance.fadeImg, GameManager.Instance.Controller.RandomizeEnemies));
    }

    /// <summary>
    /// fade image in front of player to transparent.
    /// </summary>
    public void FadeToTransparent()
    {
        if (UIManager.Instance.fadeCo != null)
        {
            V.StopCoroutine(UIManager.Instance.fadeCo);
        }
        Fade.isFading = false;
        UIManager.Instance.fadeCo = V.StartCoroutine(Fade.FadeTo(0.0f, 1.5f, UIManager.Instance.fadeImg));
    }

    /// <summary>
    /// Spawn animals randomly.
    /// </summary>
    /// <param name="cooldown">Cooldown between spawns.</param>
    public void RandomAnimal(float cooldown)
    {
        GameModel.currentTime = Time.time;
        if (GameModel.currentTime - GameModel.imaginationSpawnCooldown > GameModel.imaginationLastSpawnTime)
        {
            GameModel.imaginationLastSpawnTime = GameModel.currentTime;
            Vector3 temp = UnityEngine.Random.insideUnitSphere;
            if (temp.y < 0)
            {
                temp.y = 0;
            }

            Pose tempPose = new Pose(temp * GameModel.MAX_SPAWN_DISTANCE, Quaternion.identity);
            //var anchor = Session.CreateAnchor(tempPose);
            
            AnimalPool.Instance.Spawn(tempPose.position);
        }
    }

    /// <summary>
    /// Spawn enemies randomly.
    /// </summary>
    /// <param name="cooldown">Cooldown between spawns.</param>
    public void RandomEnemy(float cooldown)
    {
        GameModel.currentTime = Time.time;
        if (GameModel.currentTime - GameModel.enemySpawnCooldown > GameModel.enemyLastSpawnTime)
        {
            GameModel.enemyLastSpawnTime = GameModel.currentTime;
            Vector3 temp = UnityEngine.Random.insideUnitSphere;
            if (temp.y < 0)
            {
                temp.y = 0;
            }

            Pose tempPose = new Pose(temp * GameModel.MAX_SPAWN_DISTANCE, Quaternion.identity);
            //var anchor = Session.CreateAnchor(tempPose);
            MonsterPool.Instance.Spawn(tempPose.position);
        }
    }

    /// <summary>
    /// Spawn phenomenons randomly.
    /// </summary>
    /// <param name="cooldown">Cooldown between spawns.</param>
    public void RandomPhenomenon(float cooldown)
    {
        GameModel.currentTime = Time.time;
        if (GameModel.currentTime - GameModel.phenomenonSpawnCooldown > GameModel.phenomenonLastSpawnTime)
        {
            GameModel.phenomenonLastSpawnTime = GameModel.currentTime;
            Vector3 temp = UnityEngine.Random.insideUnitSphere;

            if (temp.y < 0)
            {
                temp.y = 0;
            }

            Pose tempPose = new Pose(temp * GameModel.MAX_SPAWN_DISTANCE, Quaternion.identity);
            PhenomenonPool.Instance.Spawn(tempPose.position);
        }
    }

    /// <summary>
    /// Spawns a boss infront of player with GameModel.bossSpawnCooldown as interval.
    /// </summary>
    /// <param name="cooldown"></param>
    public void SpawnBoss(float cooldown)
    {
        if (ScoreManager.Instance.sessionImaginationPoints <= M.bossSpawnTriggerIP)
            return;

        if (GameModel.currentTime - GameModel.bossSpawnCooldown > GameModel.bossLastSpawnTime)
        {
            GameModel.bossLastSpawnTime = Time.time;
            Camera cam = ARCoreView.Instance.Model.FirstPersonCamera;
            Vector3 bossSpawnPos = cam.transform.position + cam.transform.forward * M.bossSpawnDistance;
            bossSpawnPos.y = 0.0f;
            GameObject.Instantiate(M.bossPrefab, bossSpawnPos, Quaternion.identity);
        }
    }
    
    /// <summary>
    /// Randomize enemies position if the player stares at an enemy for too long.
    /// </summary>
    public void RandomizeEnemies()
    {
        for(int i = 0; i < MonsterPool.Instance.pool.Count; i++)
        {
            Vector3 temp = UnityEngine.Random.insideUnitSphere;
            if (temp.y < 0)
            {
                temp.y = 0;
            }
            MonsterPool.Instance.pool[i].transform.position = new Vector3(temp.x, temp.y, temp.z);
        }
    }

    /// <summary>
    /// Save game details:
    /// 1. Score
    /// 2. Imagination Points
    /// 3. State
    /// 4. Volume on/off
    /// </summary>
    public void SaveGame()
    {
        M.isSaving = true;
        onSave?.Invoke();
        PlayerPrefs.SetInt("Score", ScoreManager.Instance.totalScore);
        PlayerPrefs.SetInt("ImaginationPoints", ScoreManager.Instance.totalImaginationPoints);
        PlayerPrefs.SetString("State", GameManager.Instance.Model.currentState.ToString());

        // volume, 0 = muted, 1 = listening
        int mute = AudioListener.pause ? 1 : 0;
        PlayerPrefs.SetInt("Mute", mute);

    }

    /// <summary>
    /// Load game details:
    /// 1. Score
    /// 2. Imagination Points
    /// 3. State, state = player at start game.
    /// 4. Volume on/off , volume = on at start game.
    /// </summary>
    public void LoadGame()
    {
        int score = PlayerPrefs.GetInt("Score");
        int ip = PlayerPrefs.GetInt("ImaginationPoints");
        string state = PlayerPrefs.GetString("State");
        int mute = PlayerPrefs.GetInt("Mute");

        ScoreManager.Instance.totalScore = score;
        ScoreManager.Instance.totalImaginationPoints = ip;

        GameManager.Instance.Model.currentState = 
            state == 
            GAME_STATE.Pet.ToString() ? GAME_STATE.Pet : GAME_STATE.Player;

        bool isMute = mute == 1 ? true : false;
        if (isMute)
            AudioManager.Instance.Mute();
    }
}
