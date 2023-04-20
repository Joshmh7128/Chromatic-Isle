using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TrackHandler : MonoBehaviour
{
    /// <summary>
    /// This script handles an INDIVIDUAL track. It has a series of properties which we can use to adjust its 
    /// qualities and uses. We'll be customizing this based on what music tracks need. These tracks are meant
    /// to work in conjunction with one another
    /// </summary>

    AudioSource audioSource; // our audio source

    private void Awake()
    {
        // make sure we get our source
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, audioSource.maxDistance);
    }
}
