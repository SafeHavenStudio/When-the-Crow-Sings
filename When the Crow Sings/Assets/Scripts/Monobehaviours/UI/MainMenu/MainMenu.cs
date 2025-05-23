using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using FMOD.Studio;

public class MainMenu : MonoBehaviour
{
    public MainMenuDebugLoadHolder mainMenuDebugLoadHolder;

    public SceneReference mainScene;
    public SceneReference cinematicScene;

    public Button sceneLoadButtonPrefab;
    public GridLayoutGroup sceneLoadButtonsHolder;

    //public List<LevelDataResource> levelDataResources;
    public AllLevels allLevels;

    public MenuButtonSelectionHandler mainMenuHomeMBSH;
    public GameObject mainMenuPage;


    public MenuSwapper realMainMenu_MenuSwapper;

    private EventInstance MainMenuTheme;

    private void Awake()
    {
        PopulateSceneLoadDebugButtons();

        MainMenuTheme = AudioManager.instance.CreateEventInstance(FMODEvents.instance.MainMenuTheme);

        mainMenuDebugLoadHolder.resourceToLoad = null; // I don't remember what this does exactly but it's important.

        updateMusic();
    }

    private void PopulateSceneLoadDebugButtons()
    {
        foreach (LevelDataResource i in allLevels.levelDataResources)
        {
            var x = Instantiate(sceneLoadButtonPrefab);
            x.transform.SetParent(sceneLoadButtonsHolder.transform, false);
            x.onClick.AddListener(() => OnSceneLoadButtonPressed(i));
            x.GetComponentInChildren<TextMeshProUGUI>().text = i.level.Name;
        }
    }

    public void OnSceneLoadButtonPressed(LevelDataResource levelDataResource)
    {
        mainMenuDebugLoadHolder.resourceToLoad = levelDataResource;
        SceneManager.LoadScene(mainScene.Name);
        updateMusic();
    }

    public void OnFirstNewGameButtonPressed()
    {
        if (SaveDataAccess.SavedDataExistsOnDisk())
        {
            realMainMenu_MenuSwapper.OpenMenu(5);
        }
        else
        {
            OnNewGameButtonPressed();
        }
            
    }

    public void OnNewGameButtonPressed()
    {
        StartCoroutine(NewGame());
        updateMusic();
    }

    public void OnContinueButtonPressed()
    {
        StartCoroutine(ContinueGame());
    }
    IEnumerator ContinueGame()
    {
        mainMenuHomeMBSH.SetButtonsInteractability(false);

        fadeToBlack.StopAllCoroutines();
        yield return StartCoroutine(fadeToBlack.FadeIn());
        if (SaveDataAccess.SavedDataExistsOnDisk())
        {
            SaveDataAccess.ReadDataFromDisk();
        }

        int levelDataIndex = SaveDataAccess.saveData.intFlags["levelDataIndex"];
        mainMenuDebugLoadHolder.resourceToLoad = allLevels.levelDataResources[levelDataIndex];
        SceneManager.LoadScene(mainScene.Name);
        updateMusic();
    }


    public FadeToBlack fadeToBlack;

    public GameObject newGameOverlay;
    IEnumerator NewGame()
    {
        mainMenuHomeMBSH.SetButtonsInteractability(false);
        //foreach (MenuButton i in newGameOverlay.GetComponentInChildren<MenuButtonSelectionHandler>().selectableButtons) i.GetComponent<Button>().interactable = false;
        newGameOverlay.GetComponentInChildren<MenuButtonSelectionHandler>().SetButtonsInteractability(false);

        fadeToBlack.StopAllCoroutines();
        yield return StartCoroutine(fadeToBlack.FadeIn());

        yield return StartCoroutine(SaveDataAccess.EraseDataFromDisk());
        SaveDataAccess.ResetSaveData();
        //mainMenuDebugLoadHolder.resourceToLoad = allLevels.levelDataResources[1];
        //SceneManager.LoadScene(mainScene.Name);
        Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_0);
    }

    public void quitGame()
    {
        Debug.Log("Why would u quit the game please don't do this");
        Application.Quit();
    }

    private void updateMusic()
    {
        PLAYBACK_STATE playbackState;
        MainMenuTheme.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            MainMenuTheme.start();
            Debug.Log("Starting main menu theme");
        }
        else
        {
            MainMenuTheme.stop(STOP_MODE.ALLOWFADEOUT);
            Debug.Log("Stopping main menu theme");
        }
    }
}
