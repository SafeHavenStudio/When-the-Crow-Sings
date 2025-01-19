using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodexEntry : MonoBehaviour
{
    public string undiscoveredName = "Unknown";
    [TextArea]
    public string undiscoveredDescription = "Nothing is known about this character yet.";


    public string characterName = "missingno.";
    [TextArea]
    public string characterDescription = "NoMessageWritten";

    public string associatedDataKey = "TestingFlag1";

    public TextMeshProUGUI characterNameLabel;
    public TextMeshProUGUI characterDescriptionLabel;

    private void OnEnable()
    {
        if (SaveDataAccess.saveData.boolFlags[associatedDataKey])
        {
            characterNameLabel.text = characterName;
            characterDescriptionLabel.text = characterDescription;
        }
        else
        {
            characterNameLabel.text = undiscoveredName;
            characterDescriptionLabel.text = undiscoveredDescription;
        }
    }

}
