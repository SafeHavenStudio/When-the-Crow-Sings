using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestMarker : MonoBehaviour
{
    public string rumor_AssociatedDataKey;
    public string discovered_AssociatedDataKey;
    public string completed_AssociatedDataKey;

    public Image imageComponent;
    public Sprite rumorImage;
    public Sprite discoveredImage;
    public Sprite completedImage;

    private void Update()
    {
        imageComponent.enabled = true;
        if (SaveDataAccess.saveData.boolFlags[completed_AssociatedDataKey])
        {
            imageComponent.sprite = completedImage;
        }
        else if (SaveDataAccess.saveData.boolFlags[discovered_AssociatedDataKey])
        {
            imageComponent.sprite = discoveredImage;
        }
        else if (SaveDataAccess.saveData.boolFlags[rumor_AssociatedDataKey])
        {
            imageComponent.sprite = rumorImage;
        }
        else
        {
            imageComponent.enabled = false;
        }
    }
}
