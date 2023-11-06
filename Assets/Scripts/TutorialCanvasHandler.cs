using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCanvasHandler : MonoBehaviour
{
    [SerializeField] CanvasGroup tutorialCanvasGroup; // the canvas group our message is displayed on
    [SerializeField] TMPro.TextMeshProUGUI messageText; // the text we write our message on
    public string currentMessage; // what is the current message we are displaying?

    public float targetCanvasAlpha, fadeRate; // what is our target canvas alpha?

    public static TutorialCanvasHandler instance; // our instance

    private void Awake()
    {
        instance = this;
    }


    public void SetMessage(string message)
    {
        // set our current message
        currentMessage = message;
        // set our text
        messageText.text = currentMessage;
    }

    private void Update()
    {
        // process our canvas fade
        ProcessCanvasFade();
    }

    void ProcessCanvasFade()
    {
        // if we have to adjust our canvas alpha, run it
        if (tutorialCanvasGroup.alpha != targetCanvasAlpha)
        {
            tutorialCanvasGroup.alpha = Mathf.MoveTowards(tutorialCanvasGroup.alpha, targetCanvasAlpha, Time.deltaTime * fadeRate);
        }
    }
}
