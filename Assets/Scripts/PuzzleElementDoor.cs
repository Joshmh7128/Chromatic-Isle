using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElementDoor : PuzzleElement
{
    [SerializeField] Vector3 targetPos; // what is the target position we move to when opened
    [SerializeField] float speed; // how fast do we move 
    [SerializeField] AudioSource movingSource, stopSource; // audio for moving and stopping
    bool canMove; // can we move?
    bool moved; // have we moved?
    [SerializeField] bool manualActivation;

    public override void Activate()
    {
        canActivate = false;
        canMove = true;
        movingSource.Play();
        movingSource.loop = true;
    }

    private void FixedUpdate()
    {
        if (manualActivation)
            Activate();

        if (canMove)
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, speed*Time.fixedDeltaTime);

        if (Vector3.Distance(transform.localPosition, targetPos) < 1)
        {
            if (!moved)
            {
                moved = true;
                movingSource.Stop();
                stopSource.Play();
            }
        }
    }
}
