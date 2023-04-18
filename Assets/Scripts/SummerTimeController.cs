using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerTimeController : MonoBehaviour
{
    // this controls the time of day. time is calculated between 0 and 10

    [SerializeField] float time, timeSpeed; // how fast does time move?
    [SerializeField] float sunDip; // how far does the sun dip to go into night time?
    [SerializeField] Transform sun; // our sun transform

    private void FixedUpdate()
    {
        // calculate the time
        CalculateTime();
        // calculate the sun position
        SunProcess();
    }

    public void CalculateTime()
    {
        time += timeSpeed * Time.fixedDeltaTime;

        if (time > 10)
            timeSpeed = -timeSpeed;

        if (time < 0)
            timeSpeed = -timeSpeed;
    }

    // rotate the sun to match the time
    void SunProcess()
    {
        // set the sun transform
        sun.rotation = Quaternion.Euler(new Vector3(time * (sunDip * 0.1f), 0, 0));
    }
}
