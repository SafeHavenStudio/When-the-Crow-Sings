using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnable : MonoBehaviour
{
    public string associatedDataKey = "";

    public enum VALUE_TYPE { BOOL, INT, STRING }
    public VALUE_TYPE valueType = VALUE_TYPE.BOOL;
    [Header("If BOOL, then fill this.")]
    public bool boolValue;
    [Header("If INT, then fill this.")]
    public int intValue;
    [Header("If STRING, then fill this.")]
    public string stringValue;

    public bool playPickupSoundOnDisable = false;
    public bool playWoodSoundOnDisable = false;

    private void Start()
    {
        ServiceLocator.Get<DynamicEnableManager>().RegisterDynamicEnable(this);

    }
    private void OnDestroy()
    {
        ServiceLocator.Get<DynamicEnableManager>().UnregisterDynamicEnable(this);
    }
}
