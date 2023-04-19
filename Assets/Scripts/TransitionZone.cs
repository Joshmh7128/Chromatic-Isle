using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionZone : MonoBehaviour
{
    [SerializeField] string targetScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(targetScene);
        }    
    }
}
