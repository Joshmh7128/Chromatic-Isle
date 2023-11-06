using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasRequestZone : MonoBehaviour
{
    [SerializeField] string tutorialMessage; // the message we send
    [SerializeField] bool oneTimeUse; // are we a one time use?

    // when the player enters
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & PlayerPrefs.GetString(gameObject.name, "false") != "true")
        {
            // show the message
            TutorialCanvasHandler.instance.SetMessage(tutorialMessage);
            TutorialCanvasHandler.instance.targetCanvasAlpha = 1.0f;

            // if we are a one time use, set our used to true
            if (oneTimeUse)
                PlayerPrefs.SetString(gameObject.name, "true");
        }
    }

    // when the player leaves
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // show the message
            TutorialCanvasHandler.instance.targetCanvasAlpha = 0f;
        }
    }
}
