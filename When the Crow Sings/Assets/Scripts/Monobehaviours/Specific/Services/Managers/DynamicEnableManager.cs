using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicEnableManager : MonoBehaviour, IService
{
    [HideInInspector]
    public List<DynamicEnable> dynamicEnables = new List<DynamicEnable>();

    public GameStateManager gameStateManager;

    private void Awake()
    {
        RegisterSelfAsService();
    }
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }

    public void RegisterDynamicEnable(DynamicEnable _dynamicEnable)
    {
        dynamicEnables.Add(_dynamicEnable);
        DynamicEnableLogic(_dynamicEnable);
    }
    public void UnregisterDynamicEnable(DynamicEnable _dynamicEnable)
    {
        dynamicEnables.Remove(_dynamicEnable);
    }

    private void Update()
    {
        DynamicEnableUpdate();
    }

    private void DynamicEnableUpdate()
    {
        foreach (DynamicEnable i in dynamicEnables)
        {
            DynamicEnableLogic(i);
        }
    }

    private void DynamicEnableLogic(DynamicEnable i)
    {
        bool newValue = false;
        switch (i.valueType)
        {
            case DynamicEnable.VALUE_TYPE.BOOL:
                if (SaveDataAccess.saveData.boolFlags.ContainsKey(i.associatedDataKey))
                    newValue = SaveDataAccess.saveData.boolFlags[i.associatedDataKey] == i.boolValue;
                else throw new System.Exception("Associated data key for " + i.ToString() + " is not valid!");
                break;
            case DynamicEnable.VALUE_TYPE.INT:
                if (SaveDataAccess.saveData.intFlags.ContainsKey(i.associatedDataKey))
                    newValue = SaveDataAccess.saveData.intFlags[i.associatedDataKey] == i.intValue;
                else throw new System.Exception("Associated data key for " + i.ToString() + " is not valid!");
                break;
            case DynamicEnable.VALUE_TYPE.STRING:
                if (SaveDataAccess.saveData.stringFlags.ContainsKey(i.associatedDataKey))
                    newValue = SaveDataAccess.saveData.stringFlags[i.associatedDataKey] == i.stringValue;
                else throw new System.Exception("Associated data key for " + i.ToString() + " is not valid!");
                break;
        }
        CheckIfPickupSoundShouldPlay(i, newValue);
        i.gameObject.SetActive(newValue);
    }

    private void CheckIfPickupSoundShouldPlay(DynamicEnable i, bool newValue)
    {
        if (newValue == false
            && i.gameObject.activeInHierarchy
            && gameStateManager.canLoad
            && i.playPickupSoundOnDisable)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ItemCollect);
        }
    }
}
