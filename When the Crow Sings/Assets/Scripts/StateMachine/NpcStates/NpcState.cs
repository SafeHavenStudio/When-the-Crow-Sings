using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class NpcState : StateMachineState
{
    public NpcController s;
    protected NpcState(NpcController component)
    {
        s = component;
    }
}
