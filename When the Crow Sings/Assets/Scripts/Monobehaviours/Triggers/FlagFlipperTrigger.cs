using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagFlipperTrigger : MonoBehaviour
{
    public string flagToFlip = "pleaseFillThisChelle";
    public Constants.FLAG_TYPES flagType = Constants.FLAG_TYPES.BOOL;
    public bool new_bool_value = false;
    public int new_int_value = 0;
    public string new_string_value = "Hello there!";
    public void FlipFlag()
    {   
        switch (flagType)
        {
            case Constants.FLAG_TYPES.BOOL:
                SaveDataAccess.SetFlag(flagToFlip, new_bool_value);
                break;
            case Constants.FLAG_TYPES.INT:
                SaveDataAccess.SetFlag(flagToFlip, new_int_value);
                break;
            case Constants.FLAG_TYPES.STRING:
                SaveDataAccess.SetFlag(flagToFlip, new_string_value);
                break;
        }
    }
}
