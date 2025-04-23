using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public static class SteamAchievements
{ 
    public static void UnlockAchievement(string id)
    {
        //Unlocks the achievement locally
        SteamUserStats.SetAchievement(id);

        //Adds this to Steam
        SteamUserStats.StoreStats();
    }
}
