using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance { get; private set; }
    public AreaMusic areaMusic;


    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference ThrowSeed { get; private set; }
    [field: SerializeField] public EventReference SeedHit { get; private set; }
    [field: SerializeField] public EventReference CrowCocophony { get; private set; }

    [field: Header("Dialogue Sounds")]
    [field: SerializeField] public EventReference Blip { get; private set; }
    [field: SerializeField] public EventReference ChanceBlip { get; private set; }
    [field: SerializeField] public EventReference AngelBlip { get; private set; }
    [field: SerializeField] public EventReference BeauBlip { get; private set; }
    [field: SerializeField] public EventReference CalebBlip { get; private set; }
    [field: SerializeField] public EventReference FaridaBlip { get; private set; }
    [field: SerializeField] public EventReference JazmyneBlip { get; private set; }
    [field: SerializeField] public EventReference BigShock { get; private set; }
    [field: SerializeField] public EventReference LesserShock { get; private set; }

    [field: Header("QTE Success Sound")]
    [field: SerializeField] public EventReference QteSucceeded { get; private set; }
    [field: Header("QTE Fail Sound")]
    [field: SerializeField] public EventReference QteFailed { get; private set; }

    [field: Header("Ambience")]
    [field: SerializeField] public EventReference HarshWind { get; private set; }
    [field: SerializeField] public EventReference LightWind { get; private set; }
    [field: SerializeField] public EventReference PowerRoomAmbience { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference AreaMusic { get; private set; }
    [field: SerializeField] public EventReference Ambience { get; private set; }
    [field: SerializeField] public EventReference MainMenuTheme { get; private set; }
    [field: SerializeField] public EventReference PhilomenasTheme { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogError("Multiple FMODEvents instances detected!");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("FMODEvents initialized.");
    }

   //Dynamic assignment of music and ambience based off the areamusic object that sits under the leveldata object in each scene
    public void SetDynamicAssignment(EventReference music, EventReference ambience)
    {
        AreaMusic = music;
        Ambience = ambience;

        //Debug.Log($"Dynamic assignment: Music = {music.Path}, Ambience = {ambience.Path}");
    }
}
