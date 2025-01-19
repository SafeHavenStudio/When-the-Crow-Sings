using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class EnableNavmeshOnRun : MonoBehaviour
{
    private void Awake()
    {
        foreach (NavMeshSurface i in GetComponents<NavMeshSurface>()) i.enabled = true;
    }
}
