using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GoogleARCore.Examples.Common;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARCoreView : MonoBehaviour
{
    private static ARCoreView _instance;
    public static ARCoreView Instance { get { return _instance; } }

    [SerializeField] private ARCoreModel _model = new ARCoreModel();
    public ARCoreModel Model { get { return _model; } }

    private ARCoreController _controller = new ARCoreController();
    public ARCoreController Controller { get { return _controller; } }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
        }
        Instance.Controller.Awake();
    }

    void Start()
    {
        Instance.Controller.Start();
    }

    void Update()
    {
        Instance.Controller.Update();
    }

    void FixedUpdate()
    {
        Instance.Controller.FixedUpdate();
    }

    /// <summary>
    /// Use in Collect button in hierarchy.
    /// </summary>
    public void RaycastCheck()
    {
        Instance.Controller.RaycastCheck();
    }
}


