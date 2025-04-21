using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonobehaviorLifecycleEventUpdate : MonobehaviorLifecycleEvents
{
    public UnityEvent onUpdateEvent;

    // Update is called once per frame
    void Update()
    {
        onUpdateEvent.Invoke();
    }
}
