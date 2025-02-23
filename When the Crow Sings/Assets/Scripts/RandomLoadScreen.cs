using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class RandomLoadScreen : MonoBehaviour
{
    public Image sourceImage;
    public List<Sprite> loadScreens;

    // Start is called before the first frame update
    void Start()
    {
        randomizeScreen();
    }

    public void randomizeScreen()
    {
        if (loadScreens.Count > 0 && sourceImage != null)
        {
            sourceImage.sprite = loadScreens[Random.Range(0, loadScreens.Count)];
            Debug.Log("Switching loading screen");
        }
        else
        {
            Debug.Log("There are no load screens available");
        }
    }
}
