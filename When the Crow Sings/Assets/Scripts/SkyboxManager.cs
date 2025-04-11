using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkyboxManager : MonoBehaviour
{
    private Material preferredSkybox;
    public Material morningSkybox;
    public Material afternoonSkybox;
    public Material nightSkybox;
    public Material blackSkybox;

    private GameStateManager state;

    private void Awake()
    {
        state = FindObjectOfType<GameStateManager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (state != null && state.currentLevelDataLVL.isExterior)
        {
            if (scene.name.Contains("Morning"))
            {
                preferredSkybox = morningSkybox;
                Debug.Log("it is Morning");
            }
            else if (scene.name.Contains("Afternoon"))
            {
                preferredSkybox = afternoonSkybox;
                Debug.Log("it is afternoon");
            }
            else if (scene.name.Contains("Night"))
            {
                preferredSkybox = nightSkybox;
                Debug.Log("it is night");
            }
            RenderSettings.skybox = preferredSkybox;
        }

        else preferredSkybox = blackSkybox;
        RenderSettings.skybox = preferredSkybox;
        Debug.Log("you are inside there is no sky");
    }
}
