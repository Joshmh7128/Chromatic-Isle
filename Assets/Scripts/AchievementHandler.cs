using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class AchievementHandler : MonoBehaviour
{
    // script handles achievements


    // Update is called once per frame
    void FixedUpdate()
    {
        // check our achievements
        ProcessAchievements();
    }

    void ProcessAchievements()
    {
        // did we complete winter?
        if (PlayerPrefs.GetString("DoorWinter", "close") == "open")
        {
            SteamUserStats.SetAchievement("Winter_Complete");
        }

        // did we complete spring?
        if (PlayerPrefs.GetString("DoorSpring", "close") == "open")
        {
            SteamUserStats.SetAchievement("Spring_Complete");
        }
        
        // did we complete summer?
        if (PlayerPrefs.GetString("DoorSummer", "close") == "open")
        {
            SteamUserStats.SetAchievement("Summer_Complete");
        }

        // did we complete fall?
        if (PlayerPrefs.GetString("DoorFallFinal", "close") == "open")
        {
            SteamUserStats.SetAchievement("Autumn_Complete");
        }
    }
}
