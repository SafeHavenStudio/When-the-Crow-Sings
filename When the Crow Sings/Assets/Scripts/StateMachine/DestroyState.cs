using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class DestroyState : StateMachineState
{
    PlayerController s;
    public DestroyState(PlayerController component)
    {
        s = component;
    }
}
