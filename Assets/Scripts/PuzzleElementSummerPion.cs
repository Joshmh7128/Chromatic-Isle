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

    bool clicked;

    private void Start()
    {
        Activate();
    }

    bool isActive; // is this active?

    public override void Activate()
    {
        isActive = !isActive;
        clicked = false;

        // always starts active, but then set our rotation based on activity
        if (isActive)
            targetSpin.z = 0;

        if (!isActive)
            targetSpin.z = 180;

        // play a rotation noise
        spinSource.PlayOneShot(spinSounds[Random.Range(0, spinSounds.Count)]);
    }

    private void FixedUpdate()
    {
        // as we update, rotate the spin dial to our target rotation
        spinDial.rotation = Quaternion.Euler(Vector3.MoveTowards(spinDial.transform.eulerAngles, targetSpin, spinSpeed * Time.fixedDeltaTime));
        // play one shot of the click
        if (clicked == false && spinDial.eulerAngles == targetSpin)
        {
            clicked = true;
            clickSource.PlayOneShot(clickSource.clip);
        }
    }
}
