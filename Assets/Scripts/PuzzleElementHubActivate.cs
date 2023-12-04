using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class PuzzleElementHubActivate : PuzzleElement
{
    [Header("DoorSummer, DoorSpring, DoorWinter, DoorFallFinal")]
    [SerializeField] string targetName; // the target we are comparing
    [SerializeField] bool manualActivation;
    [SerializeField] GameObject activateObject, deactivateObject; // objects to activate and deactivate

    private void Start()
    {
        CheckSave();
    }

    void CheckSave()
    {
        if (PlayerPrefs.GetString(targetName) != null)
        {
            if (PlayerPrefs.GetString(targetName) == "open")
            {
                Activate();
            }
        }
    }

    public override void Activate()
    {
        if (activateObject) activateObject.SetActive(true);
        if (deactivateObject) deactivateObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (manualActivation)
            Activate();
    }
}
