using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    //Keep between 1 and 3
    public Slider qteSpeedSlider;
    public TimingMeterQTE[] qtes;

    public MenuDropdown textSizeDropdown;
    public TextMeshProUGUI[] dialogueText;

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


    private void Start()
    {
        PopulateDropdown();
        textSizeDropdown.onValueChanged.AddListener(OnTextStyleChanged);

        int savedSpeed = PlayerPrefs.GetInt("qteSpeed", 1);
        qteSpeedSlider.value = savedSpeed;

        foreach (var qte in qtes)
            qte.speed = savedSpeed;

        //Handle slider changes
        qteSpeedSlider.onValueChanged.AddListener(delegate { TimingMeterSpeed(); });
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

    private void PopulateDropdown()
    {
        textSizeDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < textStyles.Count; i++)
        {
            var style = textStyles[i];
            options.Add($"Size {style.fontSize} / Spacing {style.lineSpacing}");
        }
        textSizeDropdown.AddOptions(options);
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

        textSizeDropdown.value = index;

        //Apply on load
        OnTextStyleChanged(index);
    }
}
