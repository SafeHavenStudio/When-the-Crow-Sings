using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThreeDCutscene : MonoBehaviour
{
    public CutsceneFinder cutsceneFinder;
    public CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        cutsceneFinder = FindObjectOfType<CutsceneFinder>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button14))
            {
                cutsceneFinder.fadeToBlack();
            }
        }
    }
}
