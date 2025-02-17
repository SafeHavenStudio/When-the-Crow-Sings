using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic_SCN_Manager : MonoBehaviour
{
    public List<GameObject> cutscenes = new List<GameObject>();

    public enum DesiredBehavior
    {
        NONE,
        CUTSCENE_0,
        GAME_OVER
    }
    static DesiredBehavior desiredBehavior = DesiredBehavior.CUTSCENE_0;

    public SceneReference mainScene;

    public static void LoadCinematicScene(DesiredBehavior _desiredBehavior)
    {
        desiredBehavior = _desiredBehavior;
        SceneManager.LoadScene("Cinematic_SCN"); // Not using a scenereference because it'll only ever be this one spot.
        Debug.Log("Desired behavior is " +  desiredBehavior.ToString());
    }

    private void Awake()
    {
        switch (desiredBehavior)
        {
            case DesiredBehavior.CUTSCENE_0:
                cutscenes[0].SetActive(true);
                cutscenes[0].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(LoadMain_SCN);
                break;
            case DesiredBehavior.GAME_OVER:
                Debug.Log("YOU DIED");
                break;
            default: // This should include DesiredBehavior.NONE
                throw new System.Exception("Cinematic_SCN WAS NOT CORRECTLY LOADED, PLEASE USE LoadCinematicScene() AND PASS A VALID DesiredBehavior.");
                break;
        }
    }

    private void OnDestroy()
    {
        desiredBehavior = DesiredBehavior.NONE;
    }

    public void LoadMain_SCN()
    {
        // TODO: End it.
        Debug.Log("End of cutscene.");
        SceneManager.LoadScene(mainScene.Name);
    }
}
