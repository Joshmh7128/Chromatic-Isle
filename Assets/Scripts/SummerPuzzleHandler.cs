using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummerPuzzleHandler : MonoBehaviour
{
    [SerializeField] List<PuzzleElementSummerPion> pions;
    [SerializeField] List<bool> correctAnswers; 
    [SerializeField] PuzzleElementDoor doorLeft, doorRight;

    private void FixedUpdate()
    {
        CheckSolution();
    }

    // each frame check to see if we have the solution
    void CheckSolution()
    {
        // create a new list
        List<bool> checkAnswers = new List<bool>();

        // add all the answers
        foreach(PuzzleElementSummerPion pion in pions)
            checkAnswers.Add(pion.interactable.activeStatus);

        bool solved = true;

        // check all the answers
        for (int i = 0; i < correctAnswers.Count; i++)
        {
            if (checkAnswers[i] != correctAnswers[i])
                solved = false;
        }

        // if we can reach this code, activate!
        if (solved)
        Solve();
    }

    // run when we solve the puzzle
    void Solve()
    {
        if (doorLeft.canActivate)
            doorLeft.Activate();

        if (doorRight.canActivate)
            doorRight.Activate();
    }
}
