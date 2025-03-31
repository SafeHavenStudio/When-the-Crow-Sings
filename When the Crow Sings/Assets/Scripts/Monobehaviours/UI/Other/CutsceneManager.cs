using Cinemachine;
using Eflatun.SceneReference;
using FMOD.Studio;
using FMODUnity;
using ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public float timeForNextArrowDelay = .5f;

    public GameObject virtualCameraHolder;
    public GameSignal cameraMovementFinishedSignal;
    public GameObject nextButton;
    public EventReference[] eventReferences;
    private int i = 0;

    [HideInInspector]
    public List<CinemachineVirtualCamera> virtualCameras = new List<CinemachineVirtualCamera>();


    [HideInInspector]
    public UnityEvent cutsceneFinished = new UnityEvent();

    private void Start()
    {
        virtualCameras = virtualCameraHolder.GetComponentsInChildren<CinemachineVirtualCamera>().ToList();
        Progress();
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

    public void SendToMainMenu()
    {
        Progress();
        SceneManager.LoadScene("MainMenu_SCN");
    }

    public void OnNextButtonPressed()
    {
        nextButton.SetActive(false); // A little redundant but good to be safe.
        Progress();
        
        RuntimeManager.PlayOneShot(eventReferences[i]);
        i++;
    }

    int loop = 0;
    void Progress()
    {
        if (loop >= virtualCameras.Count)
        {
            cutsceneFinished.Invoke();
        }
        else
        {
            foreach (CinemachineVirtualCamera i in virtualCameras)
            {
                i.Priority = 0;
            }
            virtualCameras[loop].Priority = 10;
            virtualCameras[loop].GetComponent<CutsceneVirtualCamera>().triggered.Invoke();
            loop++;

            StartCoroutine(DelayNextArrow());
        }   
    }


    public void PrintExampleMessage()
    {
        Debug.Log("An example message has been printed. Weep and despair.");
    }

}
