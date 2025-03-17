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

            image1.sprite = ScreenshotToSprite(image1);
        }
    }

    public void FlipRight()
    {

    }
    public void FlipLeft()
    {

    }

    public Sprite ScreenshotToSprite(Image _image)
    {
        _image.enabled = false;

        Texture2D screenshotRaw = ScreenCapture.CaptureScreenshotAsTexture();
        Texture2D screenshotProcessed = new Texture2D(screenshotRaw.width, screenshotRaw.height, TextureFormat.RGB24, false);
        screenshotProcessed.SetPixels(screenshotRaw.GetPixels());
        screenshotProcessed.Apply();
        Destroy(screenshotRaw);

        _image.enabled = true;

        Sprite screenshotSprite = Sprite.Create(screenshotProcessed, new Rect(
            -_image.rectTransform.anchoredPosition.x + 6.0f,
            -_image.rectTransform.anchoredPosition.y + 21.0f,
            (int)_image.rectTransform.sizeDelta.x, (int)_image.rectTransform.sizeDelta.y), new Vector2(0.5f, 0.5f));

        return screenshotSprite;
    }
}
