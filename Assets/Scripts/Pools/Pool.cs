using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class Pool<T> : MonoBehaviour
{
    /// <summary>
    /// Maximum objects at one time.
    /// </summary>
    public int MAX_OBJECTS;

    /// <summary>
    /// Maximum range from player before despawning.
    /// </summary>
    public float MAX_RANGE_FROM_PLAYER;

    /// <summary>
    /// Prefab to spawn.
    /// </summary>
    public List<GameObject> prefab;

    /// <summary>
    /// Object pool. If managing more then one pool, add the pool at your script.
    /// </summary>
    public List<T> pool;

    private void Start()
    {
        InstantiatePool();
    }

    /// <summary>
    /// Instantiate pool fields.
    /// </summary>
    public virtual void InstantiatePool()
    {
        Debug.LogWarning("Base.InstantiatePool() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality.");
    }

    /// <summary>
    /// Update game objects in pool to despawn, attack etc.
    /// </summary>
    public virtual void UpdatePool()
    {
        Debug.LogWarning("Base.UpdatePool() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality.");
    }

    /// <summary>
    /// Return inactive object from current pool.
    /// </summary>
    public virtual T GetObject()
    {
        Debug.LogWarning("Base.GetObject() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality. ***Returning default <T>***");
        return default;
    }

    /// <summary>
    /// Spawns a pawn at a specified position.
    /// </summary>
    public virtual void Spawn(Vector3 position)
    {
        Debug.LogWarning("Base.Spawn() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality.");
    }

    /// <summary>
    /// Spawns a pawn at a specified position, with TrackableHit
    /// </summary>
    public virtual void Spawn(TrackableHit hit)
    {
        Debug.LogWarning("Base.Spawn() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality.");
    }

    /// <summary>
    /// Reset and recyle the pawn game object and return it to the pool.
    /// </summary>
    /// <param name="go">Game object to recycle.</param>
    public virtual void Despawn(T go) 
    {
        Debug.LogWarning("Base.Despawn() called, " +
            "make sure to override this method to " +
            "implement full pooling functionality.");
    }
}
