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
        if (PlayerPrefs.GetString("DoorWinter", "close") == "open" && PlayerPrefs.GetString("WinterAchiev", "no") != "yes")
        {
            SteamUserStats.SetAchievement("Winter_Complete");
            PlayerPrefs.SetString("WinterAchiev", "yes");
        }

        // did we complete spring?
        if (PlayerPrefs.GetString("DoorSpring", "close") == "open" && PlayerPrefs.GetString("SpringAchiev", "no") != "yes")
        {
            bool res;
            SteamUserStats.GetAchievement("Spring_Complete", out res);
            Debug.Log("Spring " + res);
            SteamUserStats.SetAchievement("Spring_Complete");
            PlayerPrefs.SetString("SpringAchiev", "yes");
        }
        
        // did we complete summer?
        if (PlayerPrefs.GetString("DoorSummer", "close") == "open" && PlayerPrefs.GetString("SummerAchiev", "no") != "yes")
        {
            SteamUserStats.SetAchievement("Summer_Complete");
            PlayerPrefs.SetString("SummerAchiev", "yes");
        }

        // did we complete fall?
        if (PlayerPrefs.GetString("DoorFallFinal", "close") == "open" && PlayerPrefs.GetString("FallAchiev", "no") != "yes")
        {
            SteamUserStats.SetAchievement("Autumn_Complete");
            PlayerPrefs.SetString("AutumnAchiev", "yes");
        }
    }
}
