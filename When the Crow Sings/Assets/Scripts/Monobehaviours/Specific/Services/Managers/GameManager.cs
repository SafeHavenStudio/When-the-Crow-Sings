using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour, IService
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
        //gameStateManager.ReloadCurrentScene(0);
        Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.GAME_OVER);
    }
}
