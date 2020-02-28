using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Fade : MonoBehaviour
{
    public static bool isFading = false;

    /// <summary>
    /// Fade an image.
    /// </summary>
    /// <param name="aValue">Target alpha value.</param>
    /// <param name="aTime">Time to reach specified alpha value.</param>
    /// <param name="fadeImg">Image to fade</param>
    /// <returns></returns>
    public static IEnumerator FadeTo(float aValue, float aTime, Image fadeImg, Action callback = null)
    {
        isFading = true;
        float alpha = fadeImg.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(0, 0, 0, Mathf.Lerp(alpha, aValue, t));
            fadeImg.color = newColor;
            yield return null;
        }
        isFading = false;
        callback?.Invoke();
    }

    /// <summary>
    /// Fade a TMPPro element.
    /// </summary>
    /// <param name="aValue">Target alpha value.</param>
    /// <param name="aTime">Time to reach specified alpha value.</param>
    /// <param name="fadeTxt">Text Mesh Pro text to fade.</param>
    /// <returns></returns>
    public static IEnumerator FadeTo(float aValue, float aTime, TMPro.TextMeshProUGUI fadeTxt, Action callback = null)
    {
        isFading = true;
        float alpha = fadeTxt.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color textColor = new Color(1, 0.45f, 0, Mathf.Lerp(alpha, aValue, t));
            fadeTxt.color = textColor;
            yield return null;
        }
        isFading = false;
        callback?.Invoke();
    }

}