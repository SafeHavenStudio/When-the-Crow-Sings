using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Make abstract!
public class TaskComponent : MonoBehaviour
{
    public enum TaskState { NOT_STARTED, IN_PROGRESS, COMPLETE }

    [HideInInspector]
    public TaskState taskState = TaskState.NOT_STARTED;


    [Header("Built-In DO NOT TOUCH")]
    public GameSignal global_taskStarted;
    public GameSignal global_taskAdvanced;
    public GameSignal global_taskFinished;
}