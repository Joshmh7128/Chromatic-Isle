using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleElementDoor : PuzzleElement
{
    [SerializeField] Vector3 targetPos; // what is the target position we move to when opened
    [SerializeField] float speed; // how fast do we move 
    [SerializeField] AudioSource movingSource, stopSource; // audio for moving and stopping
    bool canMove; // can we move?

    public override void Activate()
    {
        canActivate = false;
        canMove = true;
        movingSource.Play();
        movingSource.loop = true;
    }

    private void FixedUpdate()
    {
        if (canMove)
            transform.position = Vector3.Lerp(transform.position, targetPos, speed);

        if (Vector3.Distance(transform.position, targetPos) > 1)
        {
            movingSource.Stop();
            stopSource.Play();
        }
    }
}
