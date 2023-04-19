using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    [SerializeField] CanvasGroup fadeCanvas;

    private void Awake()
    {
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
