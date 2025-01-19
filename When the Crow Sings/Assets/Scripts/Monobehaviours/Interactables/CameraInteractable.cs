using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractable : MonoBehaviour
{
    public GameSignal gameSignal;

    public void EmitCameraInteractionSignal()
    {
        gameSignal.Emit();
    }
}
