using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour, IService
{
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register(this);
    }

    private void Awake()
    {
        RegisterSelfAsService();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
