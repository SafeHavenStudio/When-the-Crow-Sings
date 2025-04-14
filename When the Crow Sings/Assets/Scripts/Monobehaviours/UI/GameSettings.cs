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
    }

    public void FindPlayerController()
    {
        playerController = ServiceLocator.Get<PlayerController>();
        if (playerController != null )
        {
            Debug.Log("Game settings found player controller");

            playerController.isAlwaysSprinting = PlayerPrefs.GetInt("sprinting", 1) != 0;
        }
        else Debug.Log("Game settings did not find player controller");

    }

    private void Start()
    {
        PopulateDropdown();
        LoadTextSize(true);
        textSizeDropdownMenu.DropdownMenuButtonPressed.AddListener(SetTextSize);


        int savedSpeed = PlayerPrefs.GetInt("qteSpeed", 4);
        qteSpeedSlider.value = savedSpeed;

        foreach (var qte in qtes)
            qte.speed = savedSpeed;

        //Handle slider changes
        qteSpeedSlider.onValueChanged.AddListener(delegate { TimingMeterSpeed(); });

        float savedTextSpeed = PlayerPrefs.GetFloat("textSpeed", 1);
        reverseSlider.invertedValue = savedTextSpeed;
        textSpeed = savedTextSpeed;

        //textSpeedSlider.onValueChanged.AddListener(delegate { ChangeTextSpeed(); });
        reverseSlider.reversedSlider.onValueChanged.AddListener(delegate { ChangeTextSpeed(); });

        Debug.Log("qteDecayToggle loaded with value: " + qteDecayToggle.isOn);

        if (qteDecayToggle != null)
        {
            qteDecayToggle.onValueChanged.RemoveAllListeners();

            // suppress callback before setting value
            suppressToggleCallback = true;

            bool decayEnabled = PlayerPrefs.GetInt("qteDecay", 1) != 0;
            qteDecayToggle.isOn = decayEnabled;

            Debug.Log("qteDecayToggle set to " + decayEnabled);

            qteDecayToggle.onValueChanged.AddListener(delegate { isDecayingCheck(); });

            suppressToggleCallback = false;

            //Manually update the QTEs after setting without triggering PlayerPrefs again
            foreach (var stirQte in stirringQte)
                stirQte.isDecaying = decayEnabled;
        }


        if (sprintingToggle != null)
        {
            sprintingToggle.isOn = PlayerPrefs.GetInt("sprinting", 1) != 0;
            sprintingToggle.onValueChanged.AddListener(delegate { isAlwaysSprintingCheck(); });
        }
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
        if (sprintingToggle == null) return;

        playerController.isAlwaysSprinting = sprintingToggle.isOn;
        PlayerPrefs.SetInt("sprinting", sprintingToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
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
