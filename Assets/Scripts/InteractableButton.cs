using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : Interactable
{
    [SerializeField] List<Renderer> renderers = new List<Renderer>(); // our renderers for any visual FX
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // when we interact, set ourselves to active
    public override void Interact()
    {
        // set to active
        activeStatus = true;

        // play our sound
        audioSource.Play();

        if (renderers.Count != 0)
            foreach (Renderer renderer in renderers)
            {
                renderer.material = Resources.Load<Material>("Materials/ActiveElement");
            }

    }
}
