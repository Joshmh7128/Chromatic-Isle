using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatButton : InteractableButton
{
    [SerializeField] Metronome metronome;
    [SerializeField] int activeBeat; // which beat is this active on?

    private void Update()
    {
        if (metronome.beat == activeBeat && activeStatus && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
}
