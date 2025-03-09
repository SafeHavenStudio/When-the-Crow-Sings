using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class StateMachine
{
    public StateMachineState currentState;
    public StateMachineState previousState;
    public Dictionary<String,StateMachineState> states = new Dictionary<String, StateMachineState>();


    MonoBehaviour s;
    public StateMachine(MonoBehaviour monoBehaviour)
    {
        s = monoBehaviour;
    }

    // "Callbacks"
    public void Update(float deltaTime)
    {
        if (currentState != null){currentState.Update(deltaTime);}
    }
    public void FixedUpdate()
    {
        if (currentState != null){currentState.FixedUpdate();}
    }
    public void OnTriggerEnter(Collider other)
    {
        if (currentState != null) { currentState.OnTriggerEnter(other); }
    }
    public void OnTriggerExit(Collider other)
    {
        if (currentState != null) { currentState.OnTriggerExit(other); }
    }



    public void Enter(StateMachineState new_state) 
    {
        if (new_state == currentState) { return;}
        if(currentState != null) { currentState.StateExited(); }
        s.StopAllCoroutines();
        previousState = currentState;
        currentState = new_state;
        currentState.StateEntered();
    }

    public void RegisterState(StateMachineState state, string stateName)
    {
        states[stateName] = state;
    }

    public void Enter(string stateName)
    {
        Enter(states[stateName]);
    }

    public void EnterPrevious()
    {
        Enter(previousState);
    }



    // Unused
    private void _enableState(StateMachineState targetState)
    {

    }
    private void _disableState(StateMachineState targetState)
    {

    }

    //public void Enter<T>(StateMachineState new_state, List<T> args) // ?????????????????????????
    //{

    //}
}
