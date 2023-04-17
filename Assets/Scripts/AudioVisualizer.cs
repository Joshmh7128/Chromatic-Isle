using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour
{
    [SerializeField] AudioSource source;
    float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    [SerializeField] float scaleMult, lerpSpeed;

    float currentUpdateTime = 0f;

    [SerializeField] float clipLoudness, loudnessMultiplier;
    float[] clipSampleData;

    [SerializeField] Renderer vRenderer; // the material we will be effecting

    private void Awake()
    {
        clipSampleData = new float[sampleDataLength];
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // calculate the loudness
        CalculateLoudness();
        // make the material move
        ProcessMaterialChanges();
    }

    // calculate how loud we are
    void CalculateLoudness()
    {
        currentUpdateTime += Time.deltaTime;

        if (currentUpdateTime > updateStep)
            currentUpdateTime = 0f;

        source.clip.GetData(clipSampleData, source.timeSamples);

        clipLoudness = 0f;

        foreach (var sample in clipSampleData)
        {
            clipLoudness += Mathf.Abs(sample);
        }

        clipLoudness /= sampleDataLength;

        clipLoudness *= loudnessMultiplier;
    }

    // now output that as the V on a material's emission intensity
    void ProcessMaterialChanges()
    {
        //vRenderer.material.SetColor("_EmissionColor", Color.Lerp(vRenderer.material.color, vRenderer.material.color * clipLoudness, lerpSpeed * Time.fixedDeltaTime));
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, clipLoudness * scaleMult, transform.localScale.z), lerpSpeed * Time.fixedDeltaTime);

    }
}
