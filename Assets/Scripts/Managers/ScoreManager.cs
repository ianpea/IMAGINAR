using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance { get { return _instance; } }

    public const int scoreMultiplier = 15;

    // Points collected.
    /// <summary>
    /// Total imagination points since first play game.
    /// </summary>
    public int totalImaginationPoints = 0;
    /// <summary>
    /// Imagination points since the start of this session.
    /// </summary>
    public int sessionImaginationPoints = 0;

    // Score is just imagination points * scoreMultiplier
    /// <summary>
    /// Total score since first play game.
    /// </summary>
    public int totalScore = 0;
    /// <summary>
    /// Score since the start of this session.
    /// </summary>
    public int sessionScore = 0;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
        else
        {
            if(_instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
    
    private void Start()
    {
        sessionImaginationPoints = 0;
        sessionScore = 0;

        GameManager.Instance.Controller.onSave += FinalUpdate;
    }

    private void Update()
    {
        UpdateImaginationPoints();
        UpdateScore();
    }

    /// <summary>
    /// Adds points to current imagination point
    /// </summary>
    /// <param name="p">Points to add.</param>
    public void Add(GAIN g)
    {
        sessionImaginationPoints += (int)g;
    }

    public void Deduct(LOSE d)
    {
        sessionImaginationPoints -= (int)d;
    }

    public void Deduct (int d)
    {
        sessionImaginationPoints -= d;
    }

    public void UpdateScore()
    {
        // prevent FinalUpdate() from changing the sessionScore below when saving.
        if (GameManager.Instance.Model.isSaving)
            return;

        sessionScore = (totalImaginationPoints + sessionImaginationPoints) * scoreMultiplier;
        totalScore = sessionScore;
        UIManager.Instance.totalScoreTMP.text = "Total score: \n" + totalScore;
        UIManager.Instance.scoreTMP.text = "Score: " + sessionScore;
    }

    public void UpdateImaginationPoints()
    {
        UIManager.Instance.imaginationPointsTMP.text = "IP: " + sessionImaginationPoints;
    }

    /// <summary>
    /// Final update on imagination points and score.
    /// </summary>
    public void FinalUpdate()
    {
        totalScore = sessionScore;
        totalImaginationPoints += sessionImaginationPoints;
    }
}

/// <summary>
/// Points given by collecting imagination.
/// </summary>
public enum GAIN : int
{
    Animal = 100,
    Monster = 150,
    Boss = 500,
    Phenomenon = 300,
}

/// <summary>
/// Points lose from pet skills, spawning ally.
/// </summary>
public enum LOSE : int
{
    PetAOE = 5,
    PetLaser = 3,
    AllySpawn = 8,
    BossAttack = 45
}

