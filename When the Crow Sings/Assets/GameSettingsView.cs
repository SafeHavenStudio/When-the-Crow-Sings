using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsView : MonoBehaviour
{
    public Slider qteSpeedSlider;
    public Slider textSpeedSlider;
    public Toggle qteDecayToggle;
    public Toggle autoRunToggle;

    public MenuDropdown textSizeDropdownMenuPopup;

    // TODO: whatever's going on with the text style struct

    // The following are only here for completion's sake while refactoring. Will not be used.
    //public List<TimingMeterQTE> timingMeterQtePrefabs;
    //public List<StirringQTE> stirringQtePrefabs;
    //public List<TextMeshProUGUI> dialogueText;

    public void UpdateViewToModel()
    {

    }


    private void Start()
    {
        PopulateTextSizeDropdown();
    }

    private void OnEnable()
    {
        UpdateViewToModel();
    }

    void PopulateTextSizeDropdown()
    {
        List<string> textSizes = new List<string>() { "Default", "Large" };
        foreach (string i in textSizes)
        {
            textSizeDropdownMenuPopup.AddDropdownButton(i);
        }
    }
}
