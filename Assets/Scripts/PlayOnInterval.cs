using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnInterval : MonoBehaviour
{
    // our float time
    [Header("Time")]
    [SerializeField] float min, max;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(Random.Range(min, max));
        gameObject.GetComponent<AudioSource>().Play();
        StartCoroutine(Timer());
    }
}
