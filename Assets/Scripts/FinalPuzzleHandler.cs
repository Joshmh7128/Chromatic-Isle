using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPuzzleHandler : MonoBehaviour
{
    [SerializeField] List<int> solution; // the solution to the puzzles

    [SerializeField] List<InteractableStatue> statues;

    bool runFinal = false;
    bool centered = false;

    public void FixedUpdate()
    {
        SolutionCheck();
        // run final?
        if (runFinal)
            RunFinal();

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
            VolumeController.instance.finaleCanvas.alpha += Time.fixedDeltaTime;
        }

    }


}
