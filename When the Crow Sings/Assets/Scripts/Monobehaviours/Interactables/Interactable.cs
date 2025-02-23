using Cinemachine;
using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public SpriteRenderer pfInteractArrow;

    public bool autoInteract = false;

    public enum PlayerResponses { NONE, FREEZE, FEAR}
    public PlayerResponses playerResponse = PlayerResponses.NONE;

    public GameSignal interactionStartedSignal;

    public void DoInteraction()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Priority = 100;
        }
        // Would have been better to just use some polymorphism, ah well at least it's clear enough.
        if (GetComponent<DialogueInteractable>())
        {
            GetComponent<DialogueInteractable>().EmitStartDialogueSignal();

            if(AudioManager.instance.penClickSound)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.Interact);
        }
        else if (GetComponent<QTEInteractable>())
        {
            GetComponent<QTEInteractable>().EmitStartQteSignal();
        }
        else if (GetComponent<CameraInteractable>())
        {
            GetComponent<CameraInteractable>().EmitCameraInteractionSignal();
        }
        else if (GetComponent<ImagePopupInteractable>())
        {
            GetComponent<ImagePopupInteractable>().EmitImagePopupSignal();
        }
        else if (GetComponent<EnemyChangeWaypointsTrigger>())
        {
            GetComponent<EnemyChangeWaypointsTrigger>().EmitChangeWaypointSignal();
        }
        else if (GetComponent<AutoSaveTrigger>())
        {
            GetComponent<AutoSaveTrigger>().AutoSave();
        }
        else if (GetComponent<FlagFlipperTrigger>())
        {
            GetComponent<FlagFlipperTrigger>().FlipFlag();
        }
        SignalArguments args = new SignalArguments();
        args.objectArgs.Add(this);
        interactionStartedSignal.Emit(args);
    }

    public void OnDialogueFinished(SignalArguments args)
    {
        EndInteraction();
    }

    void EndInteraction()
    {
        if (virtualCamera != null)
        {
            virtualCamera.Priority = 10;
        }
    }

    public void setInteractableArrow(bool enabledOrDisabled)
    {
        //if (interactArrow != null)
        pfInteractArrow.enabled = enabledOrDisabled;
    }
}
