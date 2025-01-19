using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherInterationsManager : MonoBehaviour
{
    public GameObject floatingNextButton;

    public GameSignal cameraFinished;

    public void OnCameraInteraction()
    {
        StartCoroutine(EnableFloatingNextButton());
    }


    IEnumerator EnableFloatingNextButton()
    {
        yield return new WaitForSeconds(.5f);
        floatingNextButton.SetActive(true);
    }

    public void OnFloatingNextButtonPressed()
    {
        floatingNextButton.SetActive(false);
        cameraFinished.Emit();
    }
}
