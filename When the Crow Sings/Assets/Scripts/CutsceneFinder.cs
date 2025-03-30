using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneFinder : MonoBehaviour
{
    //This goes on the black image canvas group

    public Cutscene3DInteractable cutscene;

    [HideInInspector]
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
    }

    public void fadeToBlack()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 1f));
        //Switch animation to fishing animation
    }

    public void fadeOutOfBlack()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, 0f));
        //Switch back to regular movement state and disable fishing rod if not already
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float targetAlpha)
    {
        float fadeSpeed = 2f;
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            yield return null; //Wait for the next frame
        }
    }
}
