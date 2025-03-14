using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematic_SCN_Manager : MonoBehaviour
{
    public List<GameObject> cutscenes = new List<GameObject>();
    public GameObject gameOverScreen;

    public MainMenuDebugLoadHolder mainMenuDebugLoadHolder;
    public AllLevels allLevels;

    public enum DesiredBehavior
    {
        NONE,
        CUTSCENE_0,
        CUTSCENE_NIGHT1,
        CUTSCENE_ENDING_0,
        CUTSCENE_ENDING_1,
        CUTSCENE_ENDING_2,
        GAME_OVER
    }
    static DesiredBehavior desiredBehavior = DesiredBehavior.CUTSCENE_0;

    public SceneReference mainScene;

    public static void LoadCinematicScene(DesiredBehavior _desiredBehavior)
    {
        desiredBehavior = _desiredBehavior;
        Debug.Log("Load() Desired behavior is " + desiredBehavior.ToString());
        if (desiredBehavior == DesiredBehavior.GAME_OVER)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Death);

        SceneManager.LoadScene("Cinematic_SCN"); // Not using a scenereference because it'll only ever be this one spot.
    }

    private void Start()
    {
        Debug.Log("Awake() Desired behavior is " + desiredBehavior.ToString());
        switch (desiredBehavior)
        {
            case DesiredBehavior.CUTSCENE_0:
                cutscenes[0].SetActive(true);
                cutscenes[0].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(LoadMain_SCN);

                mainMenuDebugLoadHolder.resourceToLoad = allLevels.levelDataResources[1];
                break;
            case DesiredBehavior.CUTSCENE_NIGHT1:
                SaveDataAccess.saveData.boolFlags["NightCutsceneSeen"] = true;
                
                cutscenes[1].SetActive(true);
                cutscenes[1].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(LoadMain_SCN);

                mainMenuDebugLoadHolder.resourceToLoad = allLevels.levelDataResources[2];
                break;
            case DesiredBehavior.CUTSCENE_ENDING_0:
                cutscenes[2].SetActive(true);
                cutscenes[2].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(StartCredits);
                break;
            case DesiredBehavior.CUTSCENE_ENDING_1:
                cutscenes[3].SetActive(true);
                cutscenes[3].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(StartCredits);
                break;
            case DesiredBehavior.CUTSCENE_ENDING_2:
                cutscenes[4].SetActive(true);
                cutscenes[4].GetComponent<CutsceneManager>().cutsceneFinished.AddListener(StartCredits);
                break;
            case DesiredBehavior.GAME_OVER:
                gameOverScreen.SetActive(true);
                break;
            default: // This should include DesiredBehavior.NONE
                throw new System.Exception("Cinematic_SCN WAS NOT CORRECTLY LOADED, PLEASE USE LoadCinematicScene() AND PASS A VALID DesiredBehavior.");
                break;
        }
        desiredBehavior = DesiredBehavior.NONE;
    }

    private void OnDestroy()
    {
        //desiredBehavior = DesiredBehavior.NONE;
    }


    public void OnGameOverButtonPressed()
    {
        //SaveDataAccess.ReadDataFromDisk();
        LoadMain_SCN();
    }

    public void LoadMain_SCN()
    {
        SceneManager.LoadScene(mainScene.Name);
    }

    public void StartCredits()
    {
        Debug.Log("Hey look we finished the game! Go us!");
        Debug.Log("Pretend we started the credits.");
        SceneManager.LoadScene("MainMenu_SCN");
    }
}
