using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDestroyState : StateMachineState
{
    PlayerController s;
    public PlayerDestroyState(PlayerController component)
    {
        s = component;
    }
}
