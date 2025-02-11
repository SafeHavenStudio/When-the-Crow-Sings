using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeManager : MonoBehaviour
{
    private static GlobalVolumeManager instance;
    public static Volume volume;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // Prevent duplicate volumes
            return;
        }

        instance = this;
        volume = GetComponent<Volume>();
        DontDestroyOnLoad(gameObject); // Keep across scenes
    }
}
