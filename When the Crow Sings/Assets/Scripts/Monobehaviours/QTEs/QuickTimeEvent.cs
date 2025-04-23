using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class QuickTimeEvent : MonoBehaviour
{
    public int speed = 4;
    public bool isDecaying;

    abstract public void StartQTE();
    abstract public void SucceedQTE();
    abstract public void FailQTE();
    //public GameSignal globalStartedQteSignal;
    public GameSignal globalFinishedQteSignal;
    

    public Dictionary<string,bool> flagsToFlip;
}
