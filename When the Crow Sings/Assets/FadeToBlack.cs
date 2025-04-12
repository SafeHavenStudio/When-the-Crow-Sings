using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    // Start is called before the first frame update

    public Image image;
    public float fadeSpeed;
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        while (image.color.a < 1.0f)
        {
            Color color = image.color;
            color.a += fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        while (image.color.a > 0)
        {
            Color color = image.color;
            color.a -= fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
    }
}
