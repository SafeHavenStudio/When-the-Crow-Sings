using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInteractable : MonoBehaviour
{
    //public Cinematic_SCN_Manager.DesiredBehavior behavior = Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_NIGHT1;

    public GameSignal nightCutsceneSignal;
    public void OnInteraction()
    {
        nightCutsceneSignal.Emit();
    }
}
