using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour, IService
{
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }

    private void Awake()
    {
        RegisterSelfAsService();
    }

    public GameSignal interactionFinishedSignal;

    public void FinishInteraction()
    {
        interactionFinishedSignal.Emit();
    }

    public void OnInteraction(SignalArguments args)
    {
        // if interaction is dialogue, or qte, or something else, do logic.
    }
}
