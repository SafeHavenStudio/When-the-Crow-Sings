using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievements : MonoBehaviour
{
    public string achievementID;

    private void OnEnable()
    {
        UnlockAchievement(achievementID);
    }

    public void UnlockAchievement(string id)
    {
        //Unlocks the achievement locally
        SteamUserStats.SetAchievement(id);

        //Adds this to Steam
        SteamUserStats.StoreStats();
    }
}
