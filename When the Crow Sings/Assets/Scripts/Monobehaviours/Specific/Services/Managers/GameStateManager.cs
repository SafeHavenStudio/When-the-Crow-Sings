using ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;
using System.Collections;
using UnityEngine.UI;
using Unity.Mathematics;

public class GameStateManager : MonoBehaviour, IService
{
    public AllLevels allLevels;

    public MainMenuDebugLoadHolder mainMenuDebugLoadHolder;
    public TaskManager taskManager;

    public GameObject _playerPrefab;
    [HideInInspector] public GameObject playerHolder = null;
    [HideInInspector] public GameObject playerContent = null;

    public GameSignal levelLoadStartSignal;
    public GameSignal levelLoadFinishSignal;

    public GameSignal fullyFinishedLoadSignal;

    public Canvas cameraSpaceCanvas;


    public GameObject loadScreen;
    public Image sourceImage;
    public List<Sprite> loadScreens;
    public Image loadingAnim;

    private int targetSpawnIndex = 0;
    public bool canLoad = true;

    List<LevelData> currentLevelData = new List<LevelData>();
    [HideInInspector] public LevelData currentLevelDataLVL;

    const string SCN_PATH = "Assets/Scenes/";

    // ---------------------------------------------------------------------------
    private void Awake() {RegisterSelfAsService();} public void RegisterSelfAsService() {ServiceLocator.Register<GameStateManager>(this);}
    // ---------------------------------------------------------------------------
    private void Start()
    {
        GetLoadedScenes(); // I THINK there was a reason for this to be here??

        //if (SaveDataAccess.SavedDataExists())
        //{
        //    SaveDataAccess.ReadDataFromDisk();
        //}

        LoadOnStart();

    }

    private void LoadOnStart()
    {
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
    public void OnLevelDataLoadFinished(SignalArguments args)
    {
        ValidateScenes();
        if (args.intArgs[0] == 1) // If this signal was sent by a LEVEL being loaded
        {
            SpawnPlayer();
            ServiceLocator.Get<CrowManager>().InitializeCrows(); // This may need to be moved to allow for subscenes to have crows.
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

        //playerContent.transform.position = spawnPoint.transform.position;
        playerContent.GetComponent<PlayerController>().Initialize(spawnPoint.transform);
        playerHolder.GetComponent<PlayerHolder>().playerVirtualCameraFollowPoint.transform.position = spawnPoint.transform.position;
        playerContent.GetComponent<CharacterController>().enabled = true;

        cameraSpaceCanvas.worldCamera = playerHolder.GetComponent<PlayerHolder>().overlayCamera;
        
    }


    public void LoadRoom(LevelDataResource levelDataResource)
    {
        canLoad = false;
        lastLoadedScene = levelDataResource;
        StartCoroutine(UnloadAndLoad(levelDataResource));
    }
    IEnumerator UnloadAndLoad(LevelDataResource levelDataResource)
    {
        DestroyActors();
        yield return StartCoroutine(FadeLoadingScreen(true,0f));

        // Unload previous scenes.
        yield return StartCoroutine(UnloadLoadedScenesAsync());

        // then load them all
        yield return StartCoroutine(LoadScenesAsync(GetScenesToLoad(levelDataResource)));

        yield return StartCoroutine(FadeLoadingScreen(false));
        
        fullyFinishedLoadSignal.Emit();

        SaveDataAccess.SetFlag("levelDataIndex", allLevels.levelDataResources.IndexOf(levelDataResource));
        canLoad = true;
        
    }

    IEnumerator UnloadLoadedScenesAsync()
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        
        foreach (Scene i in GetLoadedScenes())
        {
            asyncOperations.Add(SceneManager.UnloadSceneAsync(i));
        }
        while (!AsyncOperationsAreDone(asyncOperations))
        {
            yield return null;
        }
    }

    IEnumerator LoadScenesAsync(List<SceneReference> sceneReferences)
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        foreach (SceneReference i in sceneReferences)
        {
            asyncOperations.Add(SceneManager.LoadSceneAsync(i.Name, LoadSceneMode.Additive));
        }

        while (!AsyncOperationsAreDone(asyncOperations))
        {
            float progressValue = AsyncOperationsProgress(asyncOperations);//Mathf.Clamp01(asyncOperation.progress);// / 0.9f);
            //Debug.Log("Loading Progres: "+progressValue);
            yield return null;
        }
    }

    IEnumerator FadeLoadingScreen(bool fadeIn, float _delay = .25f)
    {
        yield return new WaitForSeconds(_delay);

        float fadeSpeed = 1f;
        float maxAlpha = 1f;
        if (fadeIn)
        {
            randomizeScreen();
            loadingAnim.enabled = true;
            while (loadScreen.GetComponent<CanvasGroup>().alpha < maxAlpha)
            {
                loadScreen.GetComponent<CanvasGroup>().alpha += fadeSpeed * Time.deltaTime;
                Mathf.Clamp(loadScreen.GetComponent<CanvasGroup>().alpha, 0, maxAlpha);
                yield return null;
            }
        }
        else
        {
            loadingAnim.enabled = false;
            while (loadScreen.GetComponent<CanvasGroup>().alpha > 0f)
            {
                loadScreen.GetComponent<CanvasGroup>().alpha -= fadeSpeed * Time.deltaTime;
                Mathf.Clamp(loadScreen.GetComponent<CanvasGroup>().alpha,0,maxAlpha);
                yield return null;
            }
        }
    }

    public void randomizeScreen()
    {
        if (loadScreens.Count > 0 && sourceImage != null)
        {
            sourceImage.sprite = loadScreens[UnityEngine.Random.Range(0, loadScreens.Count)];
            Debug.Log("Switching loading screen");
        }
        else
        {
            Debug.Log("There are no load screens available");
        }
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

    public void DestroyActors()
    {
        cameraSpaceCanvas.worldCamera = null;
        Destroy(playerHolder);
        taskManager.AbortQTE();
        ServiceLocator.Get<CrowManager>().crowHolder.GetComponent<CrowHolder>().DestroyCrows();
        ServiceLocator.Get<CrowManager>().birdseedManager.DestroyBirdseed();
    }

    void ValidateScenes()
    {
        currentLevelData = FindObjectsOfType<LevelData>().ToList<LevelData>(); // TODO: Investigate Object.FindObjectByType instead. BY type, not OF type.
        Validate_No_UNASSIGNED();
        Validate_ExactlyOne_LEVEL();
        SetCurrentLEVEL();
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
                        bool boolFlag = SaveDataAccess.saveData.boolFlags[ii.associatedDataKey];
                        if (ii.boolValue != boolFlag)
                        {
                            shouldContinue = true;
                        }
                    }

                    else if (ii.valueType == SubSceneLogicBase.VALUE_TYPE.INT)
                    {
                        int intFlag = SaveDataAccess.saveData.intFlags[ii.associatedDataKey];

                        if (ii.associatedOperator == SubSceneLogicBase.OPERATOR.EQUALS)
                        {
                            if (ii.intValue != intFlag) shouldContinue = true;
                        }
                        else if (ii.associatedOperator == SubSceneLogicBase.OPERATOR.LESS_THAN)
                        {
                            if (ii.intValue >= intFlag) shouldContinue = true;
                        }
                        else
                        {
                            if (ii.intValue <= intFlag) shouldContinue = true;
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
            if (SceneManager.GetSceneAt(i).name != "Main_SCN")
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

    void SetCurrentLEVEL()
    {
        foreach (LevelData i in currentLevelData)
        {
            if (i.sceneType == LevelData.SceneType.LEVEL) currentLevelDataLVL = i;
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
