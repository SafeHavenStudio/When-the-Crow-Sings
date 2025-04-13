using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonobehaviorLifecycleEvents : MonoBehaviour
{
    public UnityEvent onStartEvent;
    public UnityEvent onEnableEvent;
    public UnityEvent onDisableEvent;
    public UnityEvent<Collider> OnTriggerEnterEvent;
    public UnityEvent<Collider> OnTriggerExitEvent;

    private void Start()
    {
        onStartEvent.Invoke();
    }
    private void OnEnable()
    {
        onEnableEvent.Invoke();
    }
    private void OnDisable()
    {
        onDisableEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent.Invoke(other);
    }
}
