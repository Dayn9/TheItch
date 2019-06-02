using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamAchievement : MonoBehaviour
{
    [SerializeField] private string APIName;

    private bool achieved = false;
    private static bool request = false;

    private void Awake()
    {
        if (!request) { request = SteamUserStats.RequestCurrentStats(); }
    }

    public void Achieve()
    {
        SteamUserStats.GetAchievement(APIName, out achieved);
        if (!achieved)
        {
            SteamUserStats.SetAchievement(APIName);
            SteamUserStats.StoreStats();
            achieved = true;
        }
    }
}
