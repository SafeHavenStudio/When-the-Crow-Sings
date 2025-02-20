using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSaveTrigger : MonoBehaviour
{
    public string associatedDynamicEnableFlag = "thisShouldMatchTheDynamicEnable";
    public void AutoSave()
    {
        Debug.Log("Autosaving!");
        SaveDataAccess.WriteDataToDisk();
        SaveDataAccess.SetFlag(associatedDynamicEnableFlag,true);
        
    }
}
