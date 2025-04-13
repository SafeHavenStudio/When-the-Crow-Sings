using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavingAnimation : MonoBehaviour
{
    public GameSignal signal;
    Animator animator;
    Image image;

    private void Start()
    {
        SaveDataAccess.dataSavedSignal = signal;
        image = GetComponent<Image>();
    }

    public void StartAnimation()
    {
        animator = GetComponent<Animator>();
        image.enabled = true;
        animator.enabled = true;
        StartCoroutine(stopAnim());
    }

    public IEnumerator stopAnim()
    {
        yield return new WaitForSecondsRealtime(3f);
        image.enabled = false;
        animator.enabled = false;
    }
}
