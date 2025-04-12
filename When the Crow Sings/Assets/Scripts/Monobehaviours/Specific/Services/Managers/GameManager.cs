using Eflatun.SceneReference;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Cinematic_SCN_Manager;

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

    public GameStateManager gameStateManager;
    public GameObject imagePopupUi;
    public void PopupImage(SignalArguments args)
    {
        Debug.Log("Popped up image!");
        imagePopupUi.SetActive(true);
        imagePopupUi.GetComponent<ImagePopup>().image.sprite = (Sprite)args.objectArgs[0];
    }
    public void OnEnemyCaughtPlayer()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.Death);
        SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.GAME_OVER);
    }
    public void OnCutsceneSignal()
    {
        SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_NIGHT1);
    }
    public void OnCutsceneSignalEnd(int _whichEnding)
    {
        switch (_whichEnding)
        {
            case 0:
                SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_0);
                break;
            case 1:
                SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_1);
                break;
            case 2:
                SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior.CUTSCENE_ENDING_2);
                break;
        }
        
    }

    public FadeToBlack fadeToBlack;

    void SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior desiredBehavior)
    {
        StartCoroutine(_SwitchToCinematicScene(desiredBehavior));
    }
    IEnumerator _SwitchToCinematicScene(Cinematic_SCN_Manager.DesiredBehavior desiredBehavior)
    {
        gameStateManager.DestroyActors();
        yield return StartCoroutine(fadeToBlack.FadeIn());
        Cinematic_SCN_Manager.LoadCinematicScene(desiredBehavior);
    }
}
