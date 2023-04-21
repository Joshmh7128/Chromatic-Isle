using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableStatue : InteractableButton
{
    [SerializeField] Material active, inactive; // our active and inactive materials
    [SerializeField] Vector3 targetRot; // our target euler rot
    [SerializeField] float rotSpeed; // our rotation speed
    [SerializeField] AudioSource stoneSource; // sound that plays on turn
    [SerializeField] List<AudioClip> stoneSounds;
    [SerializeField] List<AudioSource> musicSounds; // our music sounds that we play

    [SerializeField] string prefCheck; // which preferences do we check so we can play the correct note?
    
    public int statueState; // our current state

    private void Start()
    {
        statueState = PlayerPrefs.GetInt(gameObject.name, statueState);

        targetRot.y = 90 * statueState;

        // run a statecheck at the start so that we play the correct sound
        StateCheck();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(targetRot), rotSpeed * Time.fixedDeltaTime);
    }

    public override void Interact()
    {

        // run our base interact
        base.Interact();

        // now whenever we rotate our statue, mute all but the current statue state's audio emitter
        if (statueState < 4)
        {
            // add to the state
            statueState++;

            // if our prefcheck fails, we cannot use our true sound
            if (!PrefCheck(prefCheck) && statueState == 1)
                statueState++;

            // rotate the object by 90 degrees every time we click it
            targetRot = new Vector3(0, targetRot.y + 90, 0);

        }

        // reset at 4
        if (statueState >= 4)
            statueState = 0;

        PlayerPrefs.SetInt(gameObject.name, statueState);

        // play our stone sliding sound
        PlaySound();

        // check which state we are in and play the correct sound
        StateCheck();
    }

    bool PrefCheck(string check)
    {
        return PlayerPrefs.GetString(check, "close") == "open";
    }

    // check which state we are in and play the according sound
    void StateCheck()
    {
        for (int i = 0; i < musicSounds.Count; i++)
        {
            musicSounds[i].mute = true;

            // if this is the correct state, unmute
            if (i == statueState)
                musicSounds[i].mute = false;
        }
    }

    void PlaySound()
    {
        stoneSource.PlayOneShot(stoneSounds[Random.Range(0, stoneSounds.Count)]);
    }

}
