using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    public Animator animator;

    enum NpcState { IDLE, TALKING }
    NpcState state = NpcState.IDLE;

    // Start is called before the first frame update
    void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    // Being super explicit with the API for designers' sakes, especially since UnityEvents don't seem to support enums.
    public void NpcAnimIdleStart()
    {
        Debug.Log("No i don't want to talk to you go away");
        state = NpcState.IDLE;
    }
    public void NpcAnimTalkStart()
    {
        Debug.Log("I am playing a talking animation now!");
        state = NpcState.TALKING;
    }

    public void OnDialogueEnd(SignalArguments args)
    {
        switch (state)
        {
            case NpcState.IDLE:
                break;
            case NpcState.TALKING:
                NpcAnimIdleStart();
                break;
        }
    }
}
