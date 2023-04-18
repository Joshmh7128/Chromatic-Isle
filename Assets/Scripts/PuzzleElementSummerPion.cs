using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElementSummerPion : PuzzleElement
{
    [SerializeField] Transform spinDial; // shows our current status
    [SerializeField] Vector3 targetSpin; // the euler angles of what our dial wants to be at
    [SerializeField] List<AudioClip> spinSounds; // our spinning clips
    [SerializeField] AudioSource spinSource, clickSource;
    [SerializeField] float spinSpeed; // how fast does the dial spin?
    [SerializeField] Interactable interactable;


    bool sound; // should we play sound?

    bool clicked;

    private void Start()
    {
        isActive = interactable.activeStatus;
        Activate();
        sound = true; // so that we don't play sound on load
    }

    bool isActive; // is this active?

    public override void Activate()
    {
        isActive = !isActive;
        clicked = false;

        // always starts active, but then set our rotation based on activity
        if (isActive)
            targetSpin.y = 0;

        if (!isActive)
            targetSpin.y = 180;

        // play a rotation noise
        if (sound)
            spinSource.PlayOneShot(spinSounds[Random.Range(0, spinSounds.Count)]);
    }

    private void FixedUpdate()
    {
        // as we update, rotate the spin dial to our target rotation
        spinDial.localRotation = Quaternion.Euler(Vector3.MoveTowards(spinDial.transform.localEulerAngles, targetSpin, spinSpeed * Time.fixedDeltaTime));
        // play one shot of the click
        if (clicked == false && spinDial.eulerAngles == targetSpin && sound)
        {
            clicked = true;
            clickSource.PlayOneShot(clickSource.clip);
        }
    }
}
