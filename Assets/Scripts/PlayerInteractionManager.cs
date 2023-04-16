using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    /// <summary>
    /// handle the player's iteraction with interactable objects
    /// </summary>

    [SerializeField] Transform castStart;

    [SerializeField] RectTransform cursorNorm, cursorHighlight;

    [SerializeField] bool hoveringInteractable; // are we hovering over an interactable right now

    public void Update()
    {
        InteractionCast();
    }

    public void FixedUpdate()
    {
        AnimateCursor();
    }

    // cast forward from the player's interaction cast point and check for any interactable objects
    void InteractionCast()
    {
        RaycastHit hit;
        Physics.Raycast(castStart.position, castStart.forward, out hit, 3f, Physics.AllLayers, QueryTriggerInteraction.Collide);

        if (hit.transform != null && hit.transform.gameObject.tag == "Interactable")
        {
            if (hit.transform.gameObject.GetComponent<Interactable>().usable)
            hoveringInteractable = true;

            if (!hit.transform.gameObject.GetComponent<Interactable>().usable)
                hoveringInteractable = false;

            if (Input.GetMouseButtonDown(0) && hit.transform.gameObject.GetComponent<Interactable>().usable)
            {
                hit.transform.gameObject.GetComponent<Interactable>().Interact();
            }

        } else if (hit.transform == null)
        {
            hoveringInteractable = false;
        }

    }

    void AnimateCursor()
    {
        // if we are hovering over an interactable object, show our cursor highlight and hide our normal one
        if (hoveringInteractable)
        {
            // lerp down the normal cursor
            cursorNorm.localScale = Vector3.Lerp(cursorNorm.localScale, Vector3.zero, Time.fixedDeltaTime * 5);

            // lerp up the highlight
            cursorHighlight.localScale = Vector3.Lerp(cursorHighlight.localScale, Vector3.one, Time.fixedDeltaTime * 5);

        }

        if (!hoveringInteractable)
        {
            // lerp down the normal cursor
            cursorNorm.localScale = Vector3.Lerp(cursorNorm.localScale, Vector3.one, Time.fixedDeltaTime * 5);

            // lerp up the highlight
            cursorHighlight.localScale = Vector3.Lerp(cursorHighlight.localScale, Vector3.zero, Time.fixedDeltaTime * 5);

        }


    }
}
