using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodexEntry : MonoBehaviour
{

    public Sprite undiscoveredImage;
    public Sprite discoveredImage;

    public string undiscoveredName = "Unknown";
    public string characterName = "missingno.";

    [TextArea] public string undiscoveredDescription = "NoMessageWritten.";
    [TextArea] public string undiscoveredAdditionalDescription = "Nothing more is known at this time.";
    [TextArea] public string characterDescription = "NoMessageWritten";
    [TextArea] public string additionalDescription = "NoMessageWritten";

    public string hasBeenHeardOf = "TestingFlag1";
    public string hasBeenMetFlag = "TestingFlag1";
    public string hasBeenFinishedFlag = "TestingFlag2";

    public TextMeshProUGUI characterNameLabel;
    public TextMeshProUGUI characterDescriptionLabel;
    public TextMeshProUGUI characterDescriptionLabel2;
    public Image imageComponent;

    private void OnEnable()
    {
        if (SaveDataAccess.saveData.boolFlags[hasBeenFinishedFlag])
        {
            imageComponent.sprite = discoveredImage;

            characterNameLabel.text = characterName;
            characterDescriptionLabel.text = characterDescription;
            characterDescriptionLabel2.text = additionalDescription;
        }
        else
        {
            imageComponent.sprite = undiscoveredImage;

            characterNameLabel.text = undiscoveredName;
            characterDescriptionLabel.text = undiscoveredDescription;
            characterDescriptionLabel2.text = undiscoveredAdditionalDescription;
        }
    }

}
