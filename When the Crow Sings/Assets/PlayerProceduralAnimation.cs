using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerProceduralAnimation : MonoBehaviour
{
    public List<Rig> rigs;

    public void SetOnlyOneRigToActiveWeight(int _whichRig)
    {
        for (int i = 0; i < rigs.Count; i++)
        {
            if (i == _whichRig) rigs[i].weight = 1.0f;
            else rigs[i].weight = 0.0f;
        }


    }
}
