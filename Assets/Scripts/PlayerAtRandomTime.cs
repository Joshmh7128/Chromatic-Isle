using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtRandomTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>().loop = true;
        gameObject.GetComponent<AudioSource>().time = Random.Range(0, gameObject.GetComponent<AudioSource>().time);
    }
}
