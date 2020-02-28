//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using GoogleARCore;
using GoogleARCore.Examples.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

#if UNITY_EDITOR
// Set up touch input propagation while using Instant Preview in the editor.
using Input = GoogleARCore.InstantPreviewInput;
#endif

public class ARCoreController
{
    /// <summary>
    /// When player enters pet by moving towards it.
    /// </summary>
    public delegate void OnEnterPet();
    public OnEnterPet onEnterPet;

    public void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }

    public void Start()
    {
        PlayerInit();
    }

    /// <summary>
    /// The Unity Update() method.
    /// </summary>
    public void Update()
    {
        _UpdateApplicationLifecycle();

        // If the player has not touched the screen, we are done with this update.
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        // Should not handle input if the player is pointing on UI.
        if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
        {
            return;
        }

        AllySpawn(touch);
    }

    /// <summary>
    /// The Unity FixedUpdate() method.
    /// </summary>
    public void FixedUpdate()
    {
        
    }

    /// <summary>
    /// Check and update the application lifecycle.
    /// </summary>
    private void _UpdateApplicationLifecycle()
    {
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Only allow the screen to sleep when not tracking.
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
        }
        else
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (M.IsQuitting)
        {
            return;
        }

        // Quit if ARCore was unable to connect and give Unity some time for the toast to
        // appear.
        if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
        {
            _ShowAndroidToastMessage("Camera permission is needed to run this application.");
            M.IsQuitting = true;
            V.Invoke("_DoQuit", 0.5f);
        }
        else if (Session.Status.IsError())
        {
            _ShowAndroidToastMessage(
                "ARCore encountered a problem connecting.  Please start the app again.");
            M.IsQuitting = true;
            V.Invoke("_DoQuit", 0.5f);
        }
    }

    /// <summary>
    /// Actually quit the application.
    /// </summary>
    private void _DoQuit()
    {
        Application.Quit();
    }

    /// <summary>
    /// Show an Android toast message.
    /// </summary>
    /// <param name="message">Message string to show in the toast.</param>
    public void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity =
            unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject =
                    toastClass.CallStatic<AndroidJavaObject>(
                        "makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }

    
    ARCoreView V;
    ARCoreModel M;
    ARCoreController C;
    /// <summary>
    /// MVC setup
    /// </summary>
    private void PlayerInit()
    {
        V = ARCoreView.Instance;
        M = V.Model;
        C = V.Controller;
    }

    /// <summary>
    /// Check what kind object is in front of the camera and execute related functions.
    /// </summary>
    public void RaycastCheck()
    {
        RaycastHit hitInfo;

        if (Physics.Raycast(V.transform.position, M.FirstPersonCamera.transform.forward, out hitInfo, 100.0f))
        {
            if (GameView.Instance.Model.currentState == GAME_STATE.Player)
            {
                Debug.Log(hitInfo.collider.gameObject.tag);
                switch (hitInfo.collider.gameObject.tag)
                {
                    case "Animal": // Despawn the animal and apply particle effect.
                        ParticleManager.Instance.Play(hitInfo.collider.gameObject.GetComponent<Animal>().destroyedPFX, hitInfo.collider.gameObject.transform);
                        AnimalPool.Instance.Despawn(hitInfo.collider.gameObject.GetComponent<Animal>());
                        AudioManager.Instance.SFXCollect();
                        ScoreManager.Instance.Add(GAIN.Animal);
                        break;
                    case "Monster": // Fade the screen to black if look at the enemy too long.
                        if (!Fade.isFading)
                        {
                            GameView.Instance.Controller.FadeToBlack();
                            // volume decrease
                        }
                        break;
                    case "Phenomenon":
                        ParticleManager.Instance.Play(hitInfo.collider.gameObject.GetComponent<Phenomenon>().destroyedPFX, hitInfo.collider.gameObject.transform);
                        PhenomenonPool.Instance.Despawn(hitInfo.collider.gameObject.GetComponent<Phenomenon>());
                        AudioManager.Instance.SFXCollect();
                        ScoreManager.Instance.Add(GAIN.Phenomenon);
                        break;
                }
            }
            else if (GameView.Instance.Model.currentState == GAME_STATE.Pet)
            {
                switch (hitInfo.collider.tag)
                {
                    case "Phenomenon":
                        PhenomenonPool.Instance.Despawn(hitInfo.collider.gameObject.GetComponent<Phenomenon>());
                        AudioManager.Instance.SFXCollect();
                        ScoreManager.Instance.Add(GAIN.Phenomenon);
                        break;
                }
            }
        }
        else
        {// Cancel the fade to black if the player look away from enemy.
            GameView.Instance.Controller.FadeToTransparent();
        }
    }

    /// <summary>
    /// Spawn ally at touch position.
    /// </summary>
    /// <param name="touch"></param>
    private void AllySpawn(Touch touch)
    {
        if (GameView.Instance.Model.currentState == GAME_STATE.Pet)
            return;
        // Raycast against the location the player touched to search for planes.
        TrackableHit hit;
        TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
            TrackableHitFlags.FeaturePointWithSurfaceNormal;

        if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
        {
            // Use hit pose and camera pose to check if hittest is from the
            // back of the plane, if it is, no need to create the anchor.
            if ((hit.Trackable is DetectedPlane) &&
                Vector3.Dot(M.FirstPersonCamera.transform.position - hit.Pose.position,
                    hit.Pose.rotation * Vector3.up) < 0)
            {
                Debug.Log("Hit at back of the current DetectedPlane");
            }
            else
            {
                AllyPool.Instance.Spawn(hit);
            }
        }
    }
}

