using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    //Keep between 1 and 3
    public Slider qteSpeedSlider;
    public Slider textSpeedSlider;
    public ReverseSlider reverseSlider;
    public TimingMeterQTE[] qtes;
    public Toggle qteDecayToggle;
    public StirringQTE[] stirringQte;
    public Toggle sprintingToggle;

    public PlayerController playerController;

    public MenuDropdown textSizeDropdownMenu;

    public TextMeshProUGUI[] dialogueText;

    private bool settingsApplied = false;

    private bool suppressToggleCallback = true; //makes sure isdecayingcheck doesnt load before start

    [HideInInspector]
    public float textSpeed = 0.3f;

    [System.Serializable]
    public struct TextStyleSetting
    {
        public int fontSize;
        public float lineSpacing;

        public TextStyleSetting(int size, float spacing)
        {
            fontSize = size;
            lineSpacing = spacing;
        }
    }

    public List<TextStyleSetting> textStyles = new List<TextStyleSetting>()
    {
        new TextStyleSetting(38, -7.3f),
        new TextStyleSetting(44, -22)
    };

    private void Awake()
    {
        //FindPlayerController();
        // TODO: load the rest of the settings probably

        if (playerController != null)
        {
            ApplySavedSettings();
        }
        else
        {
            Debug.LogWarning("PlayerController is null in gameSettings Awake settings will apply later.");
        }
    }

    public void FindPlayerController()
    {
        playerController = ServiceLocator.Get<PlayerController>();
        if (playerController != null )
        {
            Debug.Log("Game settings found player controller");

            playerController.isAlwaysSprinting = PlayerPrefs.GetInt("sprinting", 1) != 0;
        }
        else Debug.LogWarning("Game settings did not find player controller");

    }

    private void Start()
    {
        PopulateDropdown();
        LoadTextSize(true);
        textSizeDropdownMenu.DropdownMenuButtonPressed.AddListener(SetTextSize);

        LoadSavedPreferences();

        StartCoroutine(WaitForPlayerController());
    }


    public void LoadSavedPreferences()
    {
        qteSpeedSlider.value = PlayerPrefs.GetInt("qteSpeed", 4);
        qteSpeedSlider.onValueChanged.AddListener(delegate { TimingMeterSpeed(); });

        float savedTextSpeed = PlayerPrefs.GetFloat("textSpeed", 1);
        reverseSlider.invertedValue = savedTextSpeed;
        reverseSlider.reversedSlider.onValueChanged.AddListener(delegate { ChangeTextSpeed(); });

        if (qteDecayToggle != null)
        {
            suppressToggleCallback = true;
            bool decayEnabled = PlayerPrefs.GetInt("qteDecay", 1) != 0;
            qteDecayToggle.isOn = decayEnabled;
            qteDecayToggle.onValueChanged.AddListener(delegate { isDecayingCheck(); });
            suppressToggleCallback = false;
        }

        if (sprintingToggle != null)
        {
            bool sprintingEnabled = PlayerPrefs.GetInt("sprinting", 1) != 0;
            sprintingToggle.isOn = sprintingEnabled;

            if (playerController != null)
            {
                playerController.isAlwaysSprinting = sprintingEnabled;
            }

            //This is really freaky I didn't know u could do this apparently it's a lambda expression which u can use to define a function or deleagate without passing local variables around
            sprintingToggle.onValueChanged.AddListener(isOn =>
            {
                if (playerController != null)
                    playerController.isAlwaysSprinting = isOn;

                PlayerPrefs.SetInt("sprinting", isOn ? 1 : 0);
                PlayerPrefs.Save();
                Debug.Log("Sprint toggle changed to: " + isOn);
            });

            Debug.Log("Sprint toggle loaded with value: " + sprintingEnabled);
        }
    }

    public IEnumerator WaitForPlayerController()
    {
        //I didn't know u could do this either
        yield return new WaitUntil(() => playerController != null);

        playerController = ServiceLocator.Get<PlayerController>();
        Debug.Log("Player controller found! Applying settings");

        //  sprint setting directly to the player
        bool sprintingEnabled = PlayerPrefs.GetInt("sprinting", 1) != 0;
        playerController.isAlwaysSprinting = sprintingEnabled;
        Debug.Log("Applied sprint toggle: " + sprintingEnabled);

        ApplySavedSettings();
    }

    public void ApplySavedSettings()
    {
        if (settingsApplied) return; //Prevent reapplying the settings multiple times

        if (playerController == null)
        {
            Debug.LogWarning("PlayerController is null when applying saved settings!");
            return;
        }

        Debug.Log("Applying saved settings");

        //Apply saved QTE decay, speed, etc.
        bool decayEnabled = PlayerPrefs.GetInt("qteDecay", 1) != 0;
        foreach (var stirQte in stirringQte)
            stirQte.isDecaying = decayEnabled;

        int savedSpeed = PlayerPrefs.GetInt("qteSpeed", 4);
        foreach (var qte in qtes)
            qte.speed = savedSpeed;

        float savedTextSpeed = PlayerPrefs.GetFloat("textSpeed", 1);
        reverseSlider.invertedValue = savedTextSpeed;
        textSpeed = savedTextSpeed;

        bool sprintingEnabled = PlayerPrefs.GetInt("sprinting", 1) != 0;
        playerController.isAlwaysSprinting = sprintingEnabled;

        if (sprintingToggle != null)
            sprintingToggle.isOn = sprintingEnabled;

        Debug.Log("Set playerController.isAlwaysSprinting to: " + sprintingEnabled);

        settingsApplied = true;
    }


    public void TimingMeterSpeed()
    {
        int newSpeed = (int)qteSpeedSlider.value;

        foreach(var qte in qtes)
            qte.speed = (int)qteSpeedSlider.value;

        PlayerPrefs.SetInt("qteSpeed", newSpeed);
        PlayerPrefs.Save();

        Debug.Log("QTE Speed set to " + newSpeed);
    }

    public void isDecayingCheck()
    {
        if (qteDecayToggle == null || suppressToggleCallback) return;

        bool decayEnabled = qteDecayToggle.isOn;

        foreach (var stirQte in stirringQte)
        {
            stirQte.isDecaying = decayEnabled;
        }

        PlayerPrefs.SetInt("qteDecay", decayEnabled ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("isdecayingcheck qteDecayToggle loaded with value: " + qteDecayToggle.isOn);
    }

    public void isAlwaysSprintingCheck()
    {
        if (sprintingToggle == null || suppressToggleCallback) return;

        // Set player sprinting state directly
        if (playerController != null)
            playerController.isAlwaysSprinting = sprintingToggle.isOn;

        PlayerPrefs.SetInt("sprinting", sprintingToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Sprinting toggle changed to: " + sprintingToggle.isOn);
    }

    private void PopulateDropdown()
    {
        //////////textSizeDropdownMenu.ClearOptions();

        List<string> options = new List<string>()
        {
            "Default",
            "Large"
        };

        foreach (string i in options)
        {
            textSizeDropdownMenu.AddDropdownButton(i);
        }
    }

    public void ChangeTextSpeed()
    {
        float newTextSpeed = reverseSlider.invertedValue;

        PlayerPrefs.SetFloat("textSpeed", + newTextSpeed);
        PlayerPrefs.Save();

        Debug.Log("Text Speed set to " + newTextSpeed);

        textSpeed = newTextSpeed;
    }

    private void SetTextSize(int index)
    {
        /* ///// TODO: EVERYTHING in this commented section needs to be reworked.
        if (index < 0 || index >= textStyles.Count)
        {
            Debug.LogWarning("Invalid style index!");
            return;
        }

        TextStyleSetting selectedStyle = textStyles[index];

        //Apply to all target text elements
        foreach (var text in dialogueText)
        {
            text.fontSize = selectedStyle.fontSize;
            text.lineSpacing = selectedStyle.lineSpacing;
        }

        Debug.Log($"Text style applied: Size {selectedStyle.fontSize}, Spacing {selectedStyle.lineSpacing}");

        */ ///// TODO: EVERYTHING in this commented section needs to be reworked.

        Debug.Log("Text size is now set to INDEX " + index.ToString());

        PlayerPrefs.SetInt("TextStyleIndex", index);
        PlayerPrefs.Save();
    }

    private void LoadTextSize(bool _setMenuGameobjects = false)
    {
        int index = PlayerPrefs.GetInt("TextStyleIndex", 0);

        if (_setMenuGameobjects )
        {
            textSizeDropdownMenu.SetCurrentlySelectedButton(index);
        }
        
        
        //Apply on load
        SetTextSize(index);
    }
}
