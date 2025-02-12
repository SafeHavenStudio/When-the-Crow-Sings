using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalVolumeManager : MonoBehaviour
{
    public static Volume volume;

    private void Awake()
    {
        volume = GetComponent<Volume>();
    }
}
