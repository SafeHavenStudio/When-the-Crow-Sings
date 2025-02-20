using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFlipperTrigger : MonoBehaviour
{
    public string flagToFlip = "thisShouldMatchTheDynamicEnable";
    public Constants.FLAG_TYPES flagType = Constants.FLAG_TYPES.BOOL;
    public bool new_bool_value = false;
    public int new_int_value = 0;
    public string new_string_value = "Hello there!";
    public void FlipFlag()
    {   
        switch (flagType)
        {
            case Constants.FLAG_TYPES.BOOL:
                SaveDataAccess.saveData.boolFlags[flagToFlip] = new_bool_value;
                break;
            case Constants.FLAG_TYPES.INT:
                SaveDataAccess.saveData.intFlags[flagToFlip] = new_int_value;
                break;
            case Constants.FLAG_TYPES.STRING:
                SaveDataAccess.saveData.stringFlags[flagToFlip] = new_string_value;
                break;
        }
    }
}
