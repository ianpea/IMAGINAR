using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ARCoreModel
{
    /// <summary>
    /// Prefab to spawn friendlies;
    /// </summary>
    public GameObject allyPrefab;

    /// <summary>
    /// The first-person camera being used to render the passthrough camera image (i.e. AR
    /// background), also represents the position of the player in the world.
    /// </summary>
    public Camera FirstPersonCamera;

    ///// <summary>
    ///// A prefab to place when a raycast from a user touch hits a vertical plane.
    ///// </summary>
    //public GameObject GameObjectVerticalPlanePrefab;

    ///// <summary>
    ///// A prefab to place when a raycast from a user touch hits a horizontal plane.
    ///// </summary>
    //public GameObject GameObjectHorizontalPlanePrefab;

    ///// <summary>
    ///// A prefab to place when a raycast from a user touch hits a feature point.
    ///// </summary>
    //public GameObject GameObjectPointPrefab;

    /// <summary>
    /// The rotation in degrees need to apply to prefab when it is placed.
    /// </summary>
    public const float k_PrefabRotation = 180.0f;

    /// <summary>
    /// True if the app is in the process of quitting due to an ARCore connection error,
    /// otherwise false.
    /// </summary>
    public bool IsQuitting = false;
}
