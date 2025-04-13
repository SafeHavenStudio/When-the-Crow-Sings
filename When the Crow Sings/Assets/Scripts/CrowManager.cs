using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowManager : MonoBehaviour, IService
{
    public CrowHolder crowHolder;
    public BirdseedManager birdseedManager;

    public void RegisterSelfAsService() {ServiceLocator.Register<CrowManager>(this);}

    void Start()
    {
        RegisterSelfAsService();
    }


    private List<CrowRestPoint> crowRestPoints = new List<CrowRestPoint>();

    public void InitializeCrows()
    {
        crowHolder.SpawnCrows(crowRestPoints);
    }

    public void RegisterCrowRestPoint(CrowRestPoint crowRestPoint)
    {
        crowRestPoints.Add(crowRestPoint);
    }
    public void UnregisterCrowRestPoint(CrowRestPoint crowRestPoint)
    {
        crowRestPoints.Remove(crowRestPoint);
    }

    public void InformCrowsOfTarget(CrowTarget crowTarget)
    {
        crowTarget.ResetRandomness();

        List<GameObject> randomSubtargets = new List<GameObject>();
        //string targets = "";
        for (int i = 0; i < crowHolder.crows.Count; i++)
        {
            randomSubtargets.Add(crowTarget.GetRandomSubtarget());
            //targets = targets + crowTarget.subTargets.IndexOf(randomSubtargets[i]).ToString() + ", ";
            crowHolder.crows[i].StartFlyingTowardBirdseed(randomSubtargets[i]);
        }
        //Debug.Log("Random Subtargets: " + targets);
    }

}
