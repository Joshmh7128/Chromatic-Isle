using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PuzzleElement : MonoBehaviour
{
    public bool canActivate = true; // can this be activated?
    public bool elementIsActive; // is this element active?

    public virtual void Activate() { } // our public activate function
}
