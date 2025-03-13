using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IService
{
    private void Awake()
    {
        RegisterSelfAsService();
    }
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }

    public GameObject imagePopupUi;
    public void PopupImage(SignalArguments args)
    {
        Debug.Log("Popped up image!");
        imagePopupUi.SetActive(true);
        imagePopupUi.GetComponent<ImagePopup>().image.sprite = (Sprite)args.objectArgs[0];
    }
    public void OnEnemyCaughtPlayer()
    {
        Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.GAME_OVER);
    }
    public void OnCutsceneSignal()
    {
        Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_NIGHT1);
    }
    public void OnCutsceneSignalEnd(int _whichEnding)
    {
        switch (_whichEnding)
        {
            case 0:
                Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_0);
                break;
            case 1:
                Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_1);
                break;
            case 2:
                Cinematic_SCN_Manager.LoadCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_2);
                break;
        }
        
    }
}
