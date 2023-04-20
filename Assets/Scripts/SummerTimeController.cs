using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SummerTimeController : MonoBehaviour
{
    // this controls the time of day. time is calculated between 0 and 10

    [SerializeField] float time, timeSpeed; // how fast does time move?
    [SerializeField] float sunDip; // how far does the sun dip to go into night time?
    [SerializeField] Transform sun; // our sun transform
    [SerializeField] Volume dayVol, nightVol; // our daytime and nighttime volumes

    [SerializeField] List<AudioSource> daySources, nightSources;

    private void FixedUpdate()
    {
        // calculate the time
        CalculateTime();
        // calculate the sun position
        SunProcess();
        // process sounds
        SoundProcess();
        // process volume weights
        VolumeProcess();
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

    void SoundProcess()
    {
        if (time < 5)
        {
            foreach (var source in daySources)
                source.volume = Mathf.Lerp(source.volume, 1, 0.25f * Time.fixedDeltaTime);
        }

        if (time > 5)
        {
            foreach (var source in daySources)
                source.volume = Mathf.Lerp(source.volume, 0, 0.25f * Time.fixedDeltaTime);
        }

        if (time > 5)
        {
            foreach (var source in nightSources)
                source.volume = Mathf.Lerp(source.volume, 1, 0.25f * Time.fixedDeltaTime);
        }

        if (time < 5)
        {
            foreach (var source in nightSources)
                source.volume = Mathf.Lerp(source.volume, 0, 0.25f * Time.fixedDeltaTime);
        } 
    }

    void VolumeProcess()
    {
        dayVol.weight = (10/time)/10;
        nightVol.weight = time/10;
    }
}
