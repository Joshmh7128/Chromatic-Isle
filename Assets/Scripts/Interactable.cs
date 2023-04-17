using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool activeStatus, usable; // is this interactable object on?

    public List<PuzzleElement> elements; // the other things we are interacting with

    public virtual void Interact() { } // our public interaction void that we will override
}
