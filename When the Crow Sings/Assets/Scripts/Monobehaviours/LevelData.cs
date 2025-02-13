using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    // TODO: Not here, but definitely find a way to more easily reference scenes than their build index/string.
    // https://www.reddit.com/r/Unity3D/comments/1888oax/i_beg_you_dont_use_the_buildindex_for_loading/ has some insights.

    public enum SceneType { UNASSIGNED, LEVEL, MAIN, VARIABLE }

    public SceneType sceneType = SceneType.UNASSIGNED;

    public bool isExterior = false;
    //public Vector2 mapCenterCoordinates = Vector2.zero;
    //public Vector2 playerMapCoordinates = Vector2.zero;

    //[Header("UNASSIGNED")]

    [Header("LEVEL")]
    [HideInInspector]
    public List<PlayerSpawnPoint> spawnPoints;

    [Header("MAIN")]
    [HideInInspector]
    public int ThisVariableDoesNothingItsJustHereToHaveSomethingForNow;


    //public LevelDataResource levelDataResource;


    public GameSignal loadingFinished;


    //private void Awake()
    //{
    //    if  (levelDataResource != null)
    //    {
    //        if (sceneType != SceneType.LEVEL)
    //        {
    //            throw new System.Exception("A non-LEVEL scene has SubScenes listed!");
    //        }

    //        foreach (SubSceneContainer i in levelDataResource.subScenes)
    //        {
    //            foreach (SubSceneLogicBase ii in i.subSceneLogics)
    //            {
    //                // TODO: Add logic to make sure nothing is broken.
    //            }
    //        }
    //    }
        
    //}
    private void Start()
    {
        SignalArguments args = new SignalArguments();
        args.intArgs.Add(0);
        
        if (sceneType == SceneType.LEVEL)
        {
            args.intArgs[0] = 1;
        }
        loadingFinished.Emit(args);
    }
}
