using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WinterBoardMarkerCheck : MonoBehaviour
{
    /// <summary>
    /// Script exists to give confirmation to the player when they make a correct beat
    /// </summary>

    [SerializeField] List<BeatButton> buttons, solution; // what buttons are we looking for?
    [SerializeField] List<Renderer> renderers; // what renderers are we working with?
    [SerializeField] AudioSource audioSource; // our trigger source

    bool played = false; 

    void SetRenderState(bool on)
    {
        if (on)
        {
            foreach (Renderer renderer in renderers)
                renderer.material = Resources.Load<Material>("Materials/ActiveElement");

            if (!played)
            {
                if (audioSource != null)
                audioSource.Play();
                played = true;
            }
        }

        if (!on)
        {
            foreach (Renderer renderer in renderers)
                renderer.material = Resources.Load<Material>("Materials/InactiveElement");

            played = false;
        }
    }

    bool ButtonStateCheck() 
    {
        // go through the interactables list and find all active interactables
        // if all active interactables are found in the solution list, the player has solved the puzzle
        List<Interactable> current = new List<Interactable>();

        foreach (Interactable interactable in buttons)
        {
            if (interactable.activeStatus)
            {
                current.Add(interactable);
            }
        }

        if (current.Count > 0)
        {
            // compare the current list to the solution list
            IEnumerable<Interactable> compared = solution.Except(current);

            // if the solution does not have one of our compared then return and do not check
            foreach (Interactable interactable in current)
            {
                if (!solution.Contains(interactable))
                {
                    return false;
                }
            }

            // directly compare and see if we have solved
            if (compared.ToList<Interactable>().Count == 0)
            {
                return true;
            }
        }

        return false;
    } 

    private void FixedUpdate()
    {
        SetRenderState(ButtonStateCheck());
    }
}
