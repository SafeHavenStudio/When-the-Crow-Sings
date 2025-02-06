using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IService
{
    // Likely where we keep track of general stuff going on in the game. Possibly birdseed.
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

    private void Update()
    {
        DynamicEnableLogic();
    }

    private void DynamicEnableLogic()
    {
        foreach (DynamicEnable i in dynamicEnables)
        {
            bool newValue = false;
            switch (i.valueType)
            {
                case DynamicEnable.VALUE_TYPE.BOOL:
                    newValue = SaveDataAccess.saveData.boolFlags[i.associatedDataKey] == i.boolValue;
                    break;
                case DynamicEnable.VALUE_TYPE.INT:
                    newValue = SaveDataAccess.saveData.intFlags[i.associatedDataKey] == i.intValue;
                    break;
                case DynamicEnable.VALUE_TYPE.STRING:
                newValue = SaveDataAccess.saveData.stringFlags[i.associatedDataKey] == i.stringValue;
                break;
            }
            if (newValue == false
                && i.gameObject.activeInHierarchy
                && gameStateManager.canLoad
                && i.playPickupSoundOnDisable)
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.ItemCollect);
                Debug.Log("Play 'pickup' sound");
            }
            i.gameObject.SetActive(newValue);
        }
    }

    public void PopupImage(SignalArguments args)
    {
        Debug.Log("Popped up image!");
    }

    public void OnEnemyCaughtPlayer()
    {
        gameStateManager.ReloadCurrentScene(0);
    }
}
