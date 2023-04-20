using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    public CanvasGroup fadeCanvas, finaleCanvas;

    public static VolumeController instance;

    private void Awake()
    {
        instance = this;
        fadeCanvas.alpha = 1;
    }

    private void FixedUpdate()
    {
        if (fadeCanvas.alpha > 0)
        {
            fadeCanvas.alpha -= Time.fixedDeltaTime;
        }
    }
}
