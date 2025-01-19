using Cinemachine;
using Eflatun.SceneReference;
using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public float timeForNextArrowDelay = .5f;

    public GameObject virtualCameraHolder;
    public GameSignal cameraMovementFinishedSignal;
    public GameObject nextButton;

    public SceneReference mainScene;

    [HideInInspector]
    public List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();

    private void Start()
    {
        virtualCameras = virtualCameraHolder.GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
        StartCoroutine(DelayNextArrow());
        //StartCoroutine(simpleProgress());
    }

    IEnumerator DelayNextArrow()
    {
        nextButton.SetActive(false);
        yield return new WaitForSeconds(timeForNextArrowDelay);
        nextButton.SetActive(true);

    }

    public void OnCameraMotionFinished()
    {
        // Doesn't actually seem like it'll be necessary unless designers want unique lengths for motion...
    }

    public void OnNextButtonPressed()
    {
        Debug.Log("NEXT");
        nextButton.SetActive(false); // A little redundant but good to be safe.
        Progress();
    }

    int loop = 0;
    void Progress()
    {
        if (loop >= virtualCameras.Count)
        {
            // TODO: End it.
            Debug.Log("End of cutscene.");
            SceneManager.LoadScene(mainScene.Name);
        }
        else
        {
            foreach (CinemachineVirtualCamera i in virtualCameras)
            {
                i.Priority = 0;
            }
            virtualCameras[loop].Priority = 10;
            loop++;

            StartCoroutine(DelayNextArrow());
        }   
    }

}
