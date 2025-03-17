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

            //SubdivideSprite(image1);
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

        //Sprite screenshotSprite = Sprite.Create(screenshotProcessed, new Rect(-image1.rectTransform.anchoredPosition.x, -image1.rectTransform.anchoredPosition.y,
        //    (int)image1.rectTransform.sizeDelta.x, (int)image1.rectTransform.sizeDelta.y), new Vector2(0.5f, 0.5f));
        Sprite screenshotSprite = Sprite.Create(screenshotProcessed, new Rect(
            -image1.rectTransform.anchoredPosition.x + ((1920-(int)image1.rectTransform.sizeDelta.x)*.5f),
            
            -image1.rectTransform.anchoredPosition.y + ((1080 - (int)image1.rectTransform.sizeDelta.y) * .5f),
            
            (int)image1.rectTransform.sizeDelta.x, (int)image1.rectTransform.sizeDelta.y), new Vector2(0.5f, 0.5f));
        Debug.Log("Sprite info: "+ screenshotSprite.rect.ToString());

        return screenshotSprite;
    }






    // Edit the vertices obtained from the sprite.  Use OverrideGeometry to
    // submit the changes.
    void SubdivideSprite(Image _image)
    {
        Sprite o = _image.sprite;
        Vector2[] sv = o.vertices;

        for (int i = 0; i < sv.Length; i++)
        {
            sv[i].x = Mathf.Clamp(
                (o.vertices[i].x - o.bounds.center.x -
                    (o.textureRectOffset.x / o.texture.width) + o.bounds.extents.x) /
                (2.0f * o.bounds.extents.x) * o.rect.width,
                0.0f, o.rect.width);

            sv[i].y = Mathf.Clamp(
                (o.vertices[i].y - o.bounds.center.y -
                    (o.textureRectOffset.y / o.texture.height) + o.bounds.extents.y) /
                (2.0f * o.bounds.extents.y) * o.rect.height,
                0.0f, o.rect.height);

            // make a small change to the 3rd vertex
            if (i == 2)
                sv[i].x = sv[i].x - 50;
        }

        o.OverrideGeometry(sv, o.triangles);
    }
}
