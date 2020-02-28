using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

/// <summary>
/// Variation of Unit.cs, but with more attribute.
/// </summary>
public class Imagination : MonoBehaviour
{
    public IMAGINATION_TYPE type;
    public Animator anim;
    public bool spawned = false;
    public IMAGINATION_STATE currenState;
    public ParticleSystem destroyedPFX;
    public ParticleSystem disappearPFX;

    public virtual void FacePlayer()
    {
        gameObject.transform.LookAt(ARCoreView.Instance.transform.position);
    }

    /// <summary>
    /// Implement your AI movement here.
    /// </summary>
    public virtual void Action() { }

    /// <summary>
    /// Implement disintegrate function here.
    /// </summary>
    public virtual void Disintegrate() { }

    /// <summary>
    /// Implement shooting function here.
    /// </summary>
    public virtual void Attack() { }
}

public enum IMAGINATION_TYPE
{
    None = 0,
    Phenomenon = 1,
    Animal = 2,
    Ally = 3
}

public enum IMAGINATION_STATE
{
    None = 0,
    Idle = 1,
    Moving = 2,
    Disintegrating = 3
}
