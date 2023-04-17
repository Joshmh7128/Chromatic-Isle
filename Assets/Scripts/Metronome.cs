using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Metronome : MonoBehaviour
{
    [SerializeField] float bpm; // our beats per minute which we calculate

    [SerializeField] List<AudioClip> beatTicks = new List<AudioClip>(); // the ticks our metronome makes as it goes beat by beat
    [SerializeField] AudioSource audioSource;

    public int beat = 1; // this counts 1 to 4 as we go through our beats

    [SerializeField] BeatBoard beatBoard;

    private void Start()
    {
        // start playing our metronome
        StartCoroutine(MetronomeCounter());
    }

    // our bpm starter
    public IEnumerator MetronomeCounter()
    {
        BeatTick();
        yield return new WaitForSecondsRealtime(60 / bpm);
        StartCoroutine(MetronomeCounter());
    }

    void BeatTick()
    {
        if (beatTicks.Count >= 4)
        {
            audioSource.PlayOneShot(beatTicks[beat - 1]); // play the correct beat
            beat++; // advance the beat
            // make sure the beat is never more than 4 or less than 1
            if (beat > 8) beat = 1;
        }
    }
}
