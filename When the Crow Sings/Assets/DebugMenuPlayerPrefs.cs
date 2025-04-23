using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMenuPlayerPrefs : MonoBehaviour
{
    public void Erase()
    {
        GameSettings.GetController().ErasePlayerPrefs();
    }

    public void Save()
    {
        GameSettings.GetController().SavePlayerPrefs();
    }

    public void Load()
    {
        GameSettings.GetController().LoadPlayerPrefs();
    }
}
