using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzleHandler : MonoBehaviour
{
    [SerializeField] List<int> solution; // the solution to the puzzles

    [SerializeField] List<InteractableStatue> statues;
    [SerializeField] List<Transform> tracks; // the tracks

    bool runFinal = false;
    bool centered = false;

    public void FixedUpdate()
    {
        SolutionCheck();
        // run final?
        if (runFinal)
            RunFinal();

        // move the tracks to the y positions of the player
        foreach (Transform track in tracks)
        {
            track.position = new Vector3(track.position.x, PlayerController.instance.transform.position.y, track.position.z);
        }

    }

    // check to see if we have solved the puzzle
    void SolutionCheck()
    {
        bool solved = true;

        for (int i = 0; i < solution.Count; i++)
        {
            if (statues[i].statueState != solution[i])
                solved = false;
        }

        if (solved)
        {
            runFinal = true;
        }
    }

    // run final
    void RunFinal()
    {
        // clear the player prefs so that the player can replay
        PlayerPrefs.DeleteAll();

        // make it so that the player can no longer move when they solve the puzzle
        PlayerController.instance.canMove = false;
        PlayerController.instance.enabled = false;
        // lerp the player to the center of the environment
    
        foreach(InteractableStatue statue in statues)
        {
            statue.usable = false;
        }
        // make all statues 2d audio
        foreach(AudioSource source in FindObjectsOfType<AudioSource>())
        {
            source.spatialBlend = 0;
        }

        if (Vector3.Distance(PlayerController.instance.transform.position, new Vector3(0, 5, 0)) < 0.5f)
        { centered = true; }

        if (centered == false)
            PlayerController.instance.transform.position = Vector3.MoveTowards(PlayerController.instance.transform.position, new Vector3(0, 5, 0), 1.5f * Time.fixedDeltaTime);

        if (centered == true)
            PlayerController.instance.transform.position = Vector3.MoveTowards(PlayerController.instance.transform.position, new Vector3(0, 45, 0), 1.5f * Time.fixedDeltaTime);

        if (PlayerController.instance.transform.position.y > 35)
        {
            SteamUserStats.SetAchievement("Hub_Complete");
            VolumeController.instance.finaleCanvas.alpha += Time.fixedDeltaTime;
            // if the player presses escape here, close the game
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Quit Requested");
                Application.Quit();
            }
        }
    }


}
