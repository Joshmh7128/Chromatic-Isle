using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using Steamworks;

public class BeatBoard : MonoBehaviour
{
    [SerializeField] GameObject tempoMarker;
    [SerializeField] List<Transform> tempoPositions;
    [SerializeField] Metronome metronome;
    [SerializeField] List<Interactable> interactables, solution; // all the buttons on the board
    public bool solved; // have we solved the board?

    [SerializeField] PuzzleElement doorRight, doorLeft;

    bool achievementChecked = false; // have we checked out achievement?

    private void Update()
    {
        // keep moving our marker
        AdvanceMarker();
        // check if we have solved the puzzle every frame
        CheckSolution();

        // if we have not gotten our achievement yet...
        if (!achievementChecked)
            CheckAchievement();
    }

    // move our marker when the beat is triggered
    public void AdvanceMarker()
    {
        // move the marker to the correct position
        tempoMarker.transform.position = tempoPositions[metronome.beat-1].position;
    }

    void CheckSolution()
    {
        // go through the interactables list and find all active interactables
        // if all active interactables are found in the solution list, the player has solved the puzzle
        List<Interactable> current = new List<Interactable>();

        foreach (Interactable interactable in interactables)
        {
            if (interactable.activeStatus)
            {
                current.Add(interactable);
            }
        }

        if (current.Count > 0)
        {
            // compare the current list to the solution list
            IEnumerable<Interactable> compared = solution.Except(current);

            // if the solution does not have one of our compared then return and do not check
            foreach (Interactable interactable in current)
            {
                if (!solution.Contains(interactable))
                {
                    solved = false;
                    return;
                }
            }

            // directly compare and see if we have solved
            if (compared.ToList<Interactable>().Count == 0)
            {
                solved = true;
                if (doorRight.canActivate)
                    doorRight.Activate();
                if (doorLeft.canActivate)
                    doorLeft.Activate();
            }
        }
    }

    // check for our achievement
    void CheckAchievement()
    {
        bool check = true;
        foreach (Interactable button in interactables)
        {
            if (button.activeStatus == false)
            {
                check = false;
                break;
            }
        }

        if (check)
        {
            SteamUserStats.SetAchievement("Winter_Bonus");

            if (achievementChecked == false)
                achievementChecked = true;
        }
    }
}
