using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePopup : MonoBehaviour
{
    public Image image;
    public GameSignal finishDialogueSignal;
    public GameObject floatingButton;

    public float secondsToWaitForButton = 1.0f;

    private void OnEnable()
    {
        StartCoroutine(enableButtonAfterSeconds());
    }

    IEnumerator enableButtonAfterSeconds()
    {
        yield return new WaitForSeconds(secondsToWaitForButton);
        floatingButton.SetActive(true);
    }
    public void OnButtonPressed()
    {
        finishDialogueSignal.Emit();
        floatingButton.SetActive(false);
        gameObject.SetActive(false);
    }
}
