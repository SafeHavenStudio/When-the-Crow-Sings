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
        GameSettingsModel model = GameSettings.GetModel();

        qteSpeedSlider.value = model.qteSpeed;
        textSpeedSlider.value = model.textSpeed;

        autoRunToggle.isOn = model.autoRun;
        qteDecayToggle.isOn = model.qteDecay;
    }


    private void Start()
    {
        PopulateTextSizeDropdown();
        ConnectCallbacksToView();
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

    void ConnectCallbacksToView()
    {
        qteSpeedSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().qteSpeed = (int)newValue;
            ViewPrint("QTE Speed set to " + GameSettings.GetModel().qteSpeed.ToString());
        });

        textSpeedSlider.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().textSpeed = newValue;
            ViewPrint("Text Speed set to " + GameSettings.GetModel().textSpeed.ToString());
        });



        autoRunToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().autoRun = newValue;
            ViewPrint("Auto-Run toggled to " + GameSettings.GetModel().autoRun.ToString());
        });

        qteDecayToggle.onValueChanged.AddListener((newValue) => {
            GameSettings.GetModel().qteDecay = newValue;
            ViewPrint("QTE Decay toggled to " + GameSettings.GetModel().qteDecay.ToString());
        });
    }

    void ViewPrint(string message)
    {
        Debug.Log(message);
    }
}
