using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class Unit : MonoBehaviour
{
    public float existTime = 0.0f;
    public float hp = 100.0f;
    public float despawnTime;
    public bool spawned = false;
    public Animator anim;
    public ParticleSystem destroyedPFX;
    public ParticleSystem disappearPFX;


    /// <summary>
    /// Reset the unit's field.
    /// </summary>
    public virtual void Reset()
    {
        existTime = 0.0f;
        spawned = false;
    }

    public virtual void FaceDirection(Vector3 lookPos)
    {
        gameObject.transform.LookAt(lookPos);
    }

    public enum UNIT_STATE
    {
        Alive = 0,
        Dead = 1,
        Idle = 2,
        Moving = 3, 
        Rotating = 4
    }
}
