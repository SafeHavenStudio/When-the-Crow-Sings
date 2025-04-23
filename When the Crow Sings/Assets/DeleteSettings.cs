using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteSettings : MonoBehaviour
{
    public void OnPressed()
    {
        GameSettings.GetController().ErasePlayerPrefs();
    }
}
