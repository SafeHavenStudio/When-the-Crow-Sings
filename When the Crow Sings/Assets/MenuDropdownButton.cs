using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdownButton : MenuButton
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Image checkImage;

    public string textName;
    public void SetButtonText(string _text)
    {
        textName = _text;
        buttonText.text = textName;
    }

    public void SetCheckImage(bool _enabled)
    {
        checkImage.enabled = _enabled;
    }
}
