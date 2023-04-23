using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinterBoardMarkerCheck : MonoBehaviour
{
    /// <summary>
    /// Script exists to give confirmation to the player when they make a correct beat
    /// </summary>

    [SerializeField] List<BeatButton> buttons; // what buttons are we looking for?
    [SerializeField] List<Renderer> renderers; // what renderers are we working with?


    void SetRenderState(bool on)
    {
        if (on)
            foreach (Renderer renderer in renderers)
                renderer.material = Resources.Load<Material>("Materials/ActiveElement");

        if (!on)
            foreach (Renderer renderer in renderers)
                renderer.material = Resources.Load<Material>("Materials/InactiveElement");
    }

    bool ButtonStateCheck() 
    {
        bool solved = true;
        foreach (BeatButton button in buttons) if (button.activeStatus == false) solved = false; 
        return solved;
    } 

    private void FixedUpdate()
    {
        SetRenderState(ButtonStateCheck());
    }
}
