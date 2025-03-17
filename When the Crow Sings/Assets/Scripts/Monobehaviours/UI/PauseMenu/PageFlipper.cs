using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageFlipper : MonoBehaviour
{
    public Image image1;
    public Image image2;


    public void TempUpdateImages()
    {
        StartCoroutine(updateImages());
        

        IEnumerator updateImages()
        {
            yield return new WaitForEndOfFrame();

            image1.sprite = ScreenshotToSprite();
        }
    }

    public void FlipRight()
    {

    }
    public void FlipLeft()
    {

    }

    public Sprite ScreenshotToSprite()
    {
        Texture2D screenshotRaw = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D screenshotProcessed = new Texture2D(screenshotRaw.width, screenshotRaw.height, TextureFormat.RGB24, false);
        screenshotProcessed.SetPixels(screenshotRaw.GetPixels());
        screenshotProcessed.Apply();
        Destroy(screenshotRaw);

        Sprite screenshotSprite = Sprite.Create(screenshotProcessed, new Rect(-image1.rectTransform.anchoredPosition.x, -image1.rectTransform.anchoredPosition.y,
            (int)image1.rectTransform.sizeDelta.x, (int)image1.rectTransform.sizeDelta.y), new Vector2(0.5f, 0.5f));

        return screenshotSprite;
    }
}
