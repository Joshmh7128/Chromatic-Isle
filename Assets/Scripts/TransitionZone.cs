using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionZone : MonoBehaviour
{
    [SerializeField] string targetScene;

    bool requestingLoad; // if we are requesting a load...

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            requestingLoad = true;
            PlayerController.instance.canMove = false; // stop the player from moving while we load the new scene
        }    
    }

    private void FixedUpdate()
    {
        if (requestingLoad)
        {
            // add to our player's fade canvas alpha
            PlayerController.instance.fadeCanvasParent.alpha += 1f * Time.deltaTime;
            // if the alpha is 1 then load a new room
            if (PlayerController.instance.fadeCanvasParent.alpha == 1)
            {
                SceneManager.LoadScene(targetScene);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            requestingLoad = false;
        }
    }
}
