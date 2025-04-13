using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdownButton : MenuButton
{
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] Image checkImage;
    public void SetButtonText(string _text)
    {
        buttonText.text = _text;
    }

    public void SetCheckImage(bool _enabled)
    {
        checkImage.enabled = _enabled;
    }
}
