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

    public IEnumerator FadeIn(bool setDefaultAlpha = true)
    {
        if (setDefaultAlpha)
        {
            Color initialColor = new Color();
            initialColor.a = 0.0f;
            image.color = initialColor;
        }

        while (image.color.a < 1.0f)
        {
            Color color = image.color;
            color.a += fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
    }

    public IEnumerator FadeOut(bool setDefaultAlpha = true)
    {
        if (setDefaultAlpha)
        {
            Color initialColor = new Color();
            initialColor.a = 1.0f;
            image.color = initialColor;
        }
        
        while (image.color.a > 0)
        {
            Color color = image.color;
            color.a -= fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
    }
}
