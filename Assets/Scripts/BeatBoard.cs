using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatBoard : MonoBehaviour
{
    [SerializeField] GameObject tempoMarker;
    [SerializeField] List<Transform> tempoPositions;
    [SerializeField] Metronome metronome;

    private void Update()
    {
        // keep moving our marker
        AdvanceMarker();
    }

    // move our marker when the beat is triggered
    public void AdvanceMarker()
    {
        // move the marker to the correct position
        tempoMarker.transform.position = tempoPositions[metronome.beat-1].position;
    }

}
