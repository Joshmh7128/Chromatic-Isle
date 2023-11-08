using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPuzzleManager : MonoBehaviour
{
    // This manages the entire fall puzzle, opens and closes doors, etc

    // our list of Audio bars for interaction
    [SerializeField] List<PuzzleElement> audioBars;

    [SerializeField] List<bool> solutionTwo, solutionThree, solutionFour; // our two solutions which the player must solve

    [SerializeField] PuzzleElementDoor doorTwoA, doorTwoB, doorThreeA, doorThreeB, doorFinalA, doorFinalB; // our doors which we activate

    bool achievementGot; // have we gotten the achievement?

    private void FixedUpdate()
    {
        CompareAnswers();

        if (!achievementGot)
        {
            AchievementCheck();
        }
    }

    // compare our answers
    void CompareAnswers()
    {
        // compare our active states to the correct answers 
        List<bool> choices = new List<bool>();

        foreach(PuzzleElement element in audioBars)
        {
            if (element.elementIsActive)
                choices.Add(true);

            if (!element.elementIsActive)
                choices.Add(false);
        }

        bool solveTwo = true, solveThree = true, solveFour = true;

        for (int i = 0; i < choices.Count; i++)
        {
            if (choices[i] != solutionTwo[i])
                solveTwo = false;

            if (choices[i] != solutionThree[i])
                solveThree = false;

            if (choices[i] != solutionFour[i])
                solveFour = false;
        }

        if (solveTwo)
        {
            if (doorTwoA.canActivate)
                doorTwoA.Activate();

            if (doorTwoB.canActivate)
                doorTwoB.Activate();
        }

        if (solveThree)
        {
            if (doorThreeA.canActivate)
                doorThreeA.Activate();

            if (doorThreeB.canActivate)
                doorThreeB.Activate();
        }

        if (solveFour)
        {
            if (doorFinalA.canActivate)
                doorFinalA.Activate();

            if (doorFinalB.canActivate)
                doorFinalB.Activate();
        }
    }

    // check for the achievement
    void AchievementCheck()
    {
        bool check = true;
        foreach (PuzzleElement bar in audioBars)
        {
            if (!bar.elementIsActive)
            {
                check = false;
                break;
            }
        }

        if (check)
        {
            SteamUserStats.SetAchievement("Fall_Bonus");
            achievementGot = true;
        }
    }
}
