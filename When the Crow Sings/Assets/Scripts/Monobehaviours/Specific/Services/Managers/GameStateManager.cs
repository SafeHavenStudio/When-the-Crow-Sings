using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using System.Collections;

public class GameStateManager : MonoBehaviour, IService
{
    public MainMenuDebugLoadHolder mainMenuDebugLoadHolder;

    public GameObject _playerPrefab;
    [HideInInspector] public GameObject playerHolder = null;
    [HideInInspector] public GameObject playerContent = null;

    public GameSignal levelLoadStartSignal;
    public GameSignal levelLoadFinishSignal;

    public GameObject loadScreen;

    private int targetSpawnIndex = 0;
    private bool canLoad = true;

    List<LevelData> currentLevelData = new List<LevelData>();

    const string SCN_PATH = "Assets/Scenes/";

    // ---------------------------------------------------------------------------
    private void Awake() {RegisterSelfAsService();} public void RegisterSelfAsService() {ServiceLocator.Register<GameStateManager>(this);}
    // ---------------------------------------------------------------------------
    private void Start()
    {
        GetLoadedScenes(); // I THINK there was a reason for this to be here??

        if (SaveData.SavedDataExists())
        {
            SaveData.ReadData();
        }

        if (mainMenuDebugLoadHolder.resourceToLoad != null)
        {
            LoadRoom(mainMenuDebugLoadHolder.resourceToLoad);
        }
        
    }
    private void Update()
    {
        //DebugLoadInput(); // Loads individual scenes via keyboard inputs. Hacky implementation of this.
    }
    // ---------------------------------------------------------------------------

    public void OnLoadStart(SignalArguments args)
    {
        if (canLoad)
        {
            if (args.objectArgs[0] is not LevelDataResource) { throw new Exception("No valid LevelDataResource assigned to the load trigger!"); }

            targetSpawnIndex = args.intArgs[0];
            LoadRoom((LevelDataResource)args.objectArgs[0]);
        }
    }
    public void OnLoadFinish(SignalArguments args)
    {
        ValidateScenes();
        if (args.intArgs[0] == 1) // If this signal was sent by a LEVEL being loaded
        {
            SpawnPlayer();
            ServiceLocator.Get<GameManager>().
                crowHolder.GetComponent<CrowHolder>().
                SpawnCrows(ServiceLocator.Get<GameManager>().crowRestPoints); // This may need to be moved to allow for subscenes to have crows.
        }
        
    }

    // ---------------------------------------------------------------------------

    void SpawnPlayer()
    {
        
        PlayerSpawnPoint spawnPoint = null;
        List<PlayerSpawnPoint> spawnPointsWithMatchingIndex = new List<PlayerSpawnPoint>();
        foreach (PlayerSpawnPoint i in FindObjectsOfType<PlayerSpawnPoint>())
        {
            if (i.entranceIndex == targetSpawnIndex)
            {
                //spawnPoint = i;
                spawnPointsWithMatchingIndex.Add(i);
            }
        }
        if (spawnPointsWithMatchingIndex.Count > 1) throw new System.Exception("More than one spawn point found with the same index.");
        if (spawnPointsWithMatchingIndex.Count < 1) throw new System.Exception("Error! No spawn point found that matches the desired index!");
        spawnPoint = spawnPointsWithMatchingIndex[0];

        playerHolder = Instantiate(_playerPrefab);
        playerContent = playerHolder.GetComponent<PlayerHolder>().playerContent;

        playerContent.transform.position = spawnPoint.transform.position;
        playerContent.GetComponent<CharacterController>().enabled = true;
    }


    public void LoadRoom(LevelDataResource levelDataResource)
    {
        lastLoadedScene = levelDataResource;

        DestroyActors();

        StartCoroutine(FadeLoadingScreen(true));

        // Unload previous scenes.
        foreach (Scene i in GetLoadedScenes())
        {
            SceneManager.UnloadSceneAsync(i); //using Async because it yells at me otherwise
        }


        // then load them all
        StartCoroutine(LoadSceneAsync(GetScenesToLoad(levelDataResource)));
    }

    IEnumerator FadeLoadingScreen(bool fadeIn)
    {
        if (fadeIn)
        {
            loadScreen.GetComponent<CanvasGroup>().alpha = 1.0f;
        }
        else
        {
            loadScreen.GetComponent<CanvasGroup>().alpha = 0f;
        }
        yield return null;
    }

    IEnumerator LoadSceneAsync(List<SceneReference> sceneReferences)
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        foreach (SceneReference i in sceneReferences)
        {
            asyncOperations.Add(SceneManager.LoadSceneAsync(i.Name, LoadSceneMode.Additive));
        }

        while (!AsyncOperationsAreDone(asyncOperations))
        {
            float progressValue = AsyncOperationsProgress(asyncOperations);//Mathf.Clamp01(asyncOperation.progress);// / 0.9f);
            Debug.Log("Loading Progres: "+progressValue);
            yield return null;
        }

        StartCoroutine(FadeLoadingScreen(false));
    }

    bool AsyncOperationsAreDone(List<AsyncOperation> asyncOperations)
    {
        foreach (AsyncOperation asyncOperation in asyncOperations)
        {
            if (!asyncOperation.isDone) return false;
        }
        return true;
    }
    float AsyncOperationsProgress(List<AsyncOperation> asyncOperations)
    {
        float totalProgress = 0.0f;
        foreach (AsyncOperation asyncOperation in asyncOperations)
        {
            totalProgress += asyncOperation.progress;
        }
        //return totalProgress; // TODO: Divide by the number of operations or something to clamp it to 100% instead of 500% or whatever it's doing here.
        float normalizedProgress = totalProgress / asyncOperations.Count;
        return normalizedProgress;
    }

    private void DestroyActors()
    {
        Destroy(playerHolder);
        ServiceLocator.Get<GameManager>().crowHolder.GetComponent<CrowHolder>().DestroyCrows();
    }

    void ValidateScenes()
    {
        currentLevelData = FindObjectsOfType<LevelData>().ToList<LevelData>(); // TODO: Investigate Object.FindObjectByType instead. BY type, not OF type.
        Validate_No_UNASSIGNED();
        Validate_ExactlyOne_LEVEL();
    }


    List<SceneReference> GetScenesToLoad(LevelDataResource levelDataResource)
    {
        List<SceneReference> scenes = new List<SceneReference>();

        scenes.Add(levelDataResource.level);

        if (levelDataResource.subScenes.Count > 0)
        {
            foreach (SubSceneContainer i in levelDataResource.subScenes)
            {
                bool shouldContinue = false;
                foreach (SubSceneLogicBase ii in i.subSceneLogics)
                {
                    if (ii.valueType == SubSceneLogicBase.VALUE_TYPE.BOOL)
                    {
                        bool boolFlag = SaveData.boolFlags[ii.associatedDataKey];
                        if (ii.boolValue != boolFlag)
                        {
                            shouldContinue = true;
                        }
                    }

                    else if (ii.valueType == SubSceneLogicBase.VALUE_TYPE.INT)
                    {
                        int intFlag = SaveData.intFlags[ii.associatedDataKey];

                        if (ii.associatedOperator == SubSceneLogicBase.OPERATOR.EQUALS)
                        {
                            if (ii.intValue != intFlag) shouldContinue = true;
                        }
                        else if (ii.associatedOperator == SubSceneLogicBase.OPERATOR.LESS_THAN)
                        {
                            if (ii.intValue! < intFlag) shouldContinue = true;
                        }
                        else
                        {
                            if (ii.intValue! > intFlag) shouldContinue = true;
                        }
                    }
                }
                if (shouldContinue) continue;
                scenes.Add(i.subScene);
            }
        }
        return scenes;
    }


    List<Scene> GetLoadedScenes()
    {
        List<Scene> scenes = new List<Scene>();
        foreach (int i in Enumerable.Range(0, SceneManager.sceneCount))
        {
            if (SceneManager.GetSceneAt(i).name != "MainScene")
            {
                scenes.Add(SceneManager.GetSceneAt(i));
            }
        }
        return scenes;
    }

    void Validate_ExactlyOne_LEVEL()
    {
        if (currentLevelData.Count(x => x.sceneType == LevelData.SceneType.LEVEL) != 1)
            throw new System.Exception("Not EXACTLY one LEVEL-type scene is currently loaded!");
    }
    void Validate_No_UNASSIGNED()
    {
        foreach (LevelData i in currentLevelData)
        {
            if (i.sceneType == LevelData.SceneType.UNASSIGNED) throw new System.Exception("Attempting to load a level of type UNASSIGNED!");
        }
    }
    [HideInInspector]
    public List<LevelDataResource> debugScenes;
    void DebugLoadInput()
    {
        if (canLoad)
        {
            LevelDataResource testResource = ScriptableObject.CreateInstance<LevelDataResource>();

            DebugLoadLevel(0,KeyCode.Alpha1,testResource);
            DebugLoadLevel(1,KeyCode.Alpha2,testResource);
            DebugLoadLevel(2,KeyCode.Alpha3,testResource);
            DebugLoadLevel(3,KeyCode.Alpha4,testResource);
            DebugLoadLevel(4,KeyCode.Alpha5,testResource);
            DebugLoadLevel(5,KeyCode.Alpha6,testResource);
            DebugLoadLevel(6,KeyCode.Alpha7,testResource);
            DebugLoadLevel(7,KeyCode.Alpha8,testResource);
            DebugLoadLevel(8,KeyCode.Alpha9,testResource);
            DebugLoadLevel(9,KeyCode.Alpha0,testResource);
        }
    }
    void DebugLoadLevel(int myIndex, KeyCode keyCode, LevelDataResource testResource)
    {
        if (Input.GetKeyUp(keyCode))
        {
            testResource = debugScenes[myIndex];
            targetSpawnIndex = 0;
            LoadRoom(testResource);
        }
    }

    LevelDataResource lastLoadedScene;
    public void ReloadCurrentScene(int spawnIndex)
    {
        if (canLoad)
        {
            targetSpawnIndex = spawnIndex;
            LoadRoom(lastLoadedScene);
        }
    }
}
