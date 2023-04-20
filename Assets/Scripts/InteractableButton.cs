using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableButton : Interactable
{
    [SerializeField] List<Renderer> renderers = new List<Renderer>(); // our renderers for any visual FX
    public AudioSource audioSource;
    [SerializeField] bool oneTimeUse; // can we only use this once?

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        RenderCheck();

    }

    // when we interact, set ourselves to active
    public override void Interact()
    {
        // set to the opposite state
        activeStatus = !activeStatus;

        if (oneTimeUse)
            usable = false;

        // play our sound
        audioSource.Play();

        RenderCheck();

        // activate the puzzle
        foreach (PuzzleElement element in elements)
        {
            element.Activate();
        }
    }

    void RenderCheck()
    {
        if (renderers.Count != 0)
            foreach (Renderer renderer in renderers)
            {
                if (activeStatus)
                    renderer.material = Resources.Load<Material>("Materials/ActiveElement");

                if (!activeStatus)
                    renderer.material = Resources.Load<Material>("Materials/InactiveElement");
            }
    }
}
