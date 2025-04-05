using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioBorders : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AdjustAspectRatio();
    }

    public void AdjustAspectRatio()
    {
        float targetAspect = 16f / 9f;
        float windowedAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowedAspect / targetAspect;
        Camera camera = GetComponent<Camera>();

        if (scaleHeight < 1f)
        {
            Rect rect = camera.rect;

            rect.width = 1f;
            rect.height = scaleHeight;
            rect.x = 0; //aligns viewport to left edge of screen
            rect.y = (1f - scaleHeight) / 2f; //center the viewport vertically and equally distribute unused space

            camera.rect = rect; //pass this info to the camera
        }
        else
        {
            float scaleWidth = 1f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1f;
            rect.x = (1f - scaleWidth) / 2f; //center the viewport horizontally
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
