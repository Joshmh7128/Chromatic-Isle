using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCanvasRequestZone : MonoBehaviour
{
    [SerializeField] string tutorialMessage; // the message we send
    [SerializeField] bool oneTimeUse; // are we a one time use?
    [SerializeField] float waitTime; // how long do we wait before it comes on
    bool playerIn;

    // when the player enters
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" & PlayerPrefs.GetString(gameObject.name, "false") != "true")
        {
            // player is in
            playerIn = true;

            // show the message
            Invoke("Show", waitTime);

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
            playerIn = false;
            // hide the message
            TutorialCanvasHandler.instance.SetMessage(tutorialMessage, 0);
        }
    }

    // function to invoke
    void Show()
    {
        if (playerIn)
            TutorialCanvasHandler.instance.SetMessage(tutorialMessage, 1);
    }
}
