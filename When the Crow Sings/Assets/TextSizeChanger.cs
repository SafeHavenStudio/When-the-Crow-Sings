using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextSizeChanger : MonoBehaviour
{
    TextMeshProUGUI tmpro;
    private void Start()
    {
        tmpro = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (GameSettings.GetModel().textSize == 0)
        {
            tmpro.fontSize = 38;
            tmpro.lineSpacing = -7.3f;
        }
        else
        {
            tmpro.fontSize = 44;
            tmpro.lineSpacing = -22;
        }
    }
}
