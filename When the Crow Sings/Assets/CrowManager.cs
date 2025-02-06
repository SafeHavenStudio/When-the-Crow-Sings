using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowManager : MonoBehaviour, IService
{
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register<CrowManager>(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        RegisterSelfAsService();
    }


    private List<CrowRestPoint> crowRestPoints = new List<CrowRestPoint>();
    public void RegisterCrowRestPoint(CrowRestPoint crowRestPoint)
    {
        crowRestPoints.Add(crowRestPoint);
    }
    public void UnregisterCrowRestPoint(CrowRestPoint crowRestPoint)
    {
        crowRestPoints.Remove(crowRestPoint);
    }
}
