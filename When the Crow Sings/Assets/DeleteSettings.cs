using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSettings : MonoBehaviour
{
    public void OnPressed()
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
