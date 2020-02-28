using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchPhase GetCurrentTouchPhase()
    {
        Touch touch;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    return TouchPhase.Began;
                case TouchPhase.Stationary:
                    return TouchPhase.Stationary;
                case TouchPhase.Ended:
                    return TouchPhase.Ended;
                default:
                    return default;
            }
        }

        return default;
    }
}
