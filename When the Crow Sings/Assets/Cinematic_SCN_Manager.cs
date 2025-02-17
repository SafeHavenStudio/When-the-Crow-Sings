using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic_SCN_Manager : MonoBehaviour
{
    public enum DesiredBehavior
    {
        NONE,
        CUTSCENE_0,
        GAME_OVER
    }
    static DesiredBehavior desiredBehavior = DesiredBehavior.CUTSCENE_0;


    public static void LoadCinematicScene(SceneReference sceneReference, DesiredBehavior _desiredBehavior)
    {
        SceneManager.LoadScene(sceneReference.Name);
        desiredBehavior = _desiredBehavior;
        Debug.Log("Desired behavior is " +  desiredBehavior.ToString());
    }

    private void OnDestroy()
    {
        desiredBehavior = DesiredBehavior.NONE;
    }
}
