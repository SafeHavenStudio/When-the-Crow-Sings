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

    public MenuDropdown textSizeDropdown;
    public TextMeshProUGUI[] dialogueText;

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
        FindPlayerController();
    }

    public void FindPlayerController()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.isAlwaysSprinting = PlayerPrefs.GetInt("sprinting", 1) != 0;
        Debug.Log("Game settings found player controller");
    }

    private void Start()
    {
        PopulateDropdown();
        ////////////textSizeDropdown.onValueChanged.AddListener(OnTextStyleChanged);

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

        foreach(var stirQte in stirringQte)
        stirQte.isDecaying = PlayerPrefs.GetInt("qteDecay", 1) != 0;

        if (qteDecayToggle != null)
        {
            qteDecayToggle.isOn = qteDecayToggle;
            qteDecayToggle.onValueChanged.AddListener(delegate { isDecayingCheck(); });
        }

        if (sprintingToggle != null)
        {
            sprintingToggle.isOn = sprintingToggle;
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
        if (qteDecayToggle == null) return;

        foreach (var stirQte in stirringQte)
        {
            stirQte.isDecaying = qteDecayToggle.isOn;
            PlayerPrefs.SetInt("qteDecay", stirQte.isDecaying ? 1 : 0);
        }
            
        PlayerPrefs.Save();
    }

    public void isAlwaysSprintingCheck()
    {
        if (sprintingToggle == null) return;

        playerController.isAlwaysSprinting = sprintingToggle.isOn;
        PlayerPrefs.SetInt("sprinting", playerController.isAlwaysSprinting ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void PopulateDropdown()
    {
        //////////textSizeDropdown.ClearOptions();

        List<string> options = new List<string>()
        {
            "Default",
            "Large"
        };

        //////////textSizeDropdown.AddOptions(options);
    }

    public void ChangeTextSpeed()
    {
        float newTextSpeed = reverseSlider.invertedValue;

        PlayerPrefs.SetFloat("textSpeed", + newTextSpeed);
        PlayerPrefs.Save();

        Debug.Log("Text Speed set to " + newTextSpeed);

        textSpeed = newTextSpeed;
    }

    private void OnTextStyleChanged(int index)
    {
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

        PlayerPrefs.SetInt("TextStyleIndex", index);
        PlayerPrefs.Save();
    }

    private void LoadTextStyle()
    {
        int index = PlayerPrefs.GetInt("TextStyleIndex", 0);

        //////////textSizeDropdown.value = index; 

        //Apply on load
        OnTextStyleChanged(index);
    }
}
