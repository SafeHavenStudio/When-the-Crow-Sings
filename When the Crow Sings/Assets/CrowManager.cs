using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowManager : MonoBehaviour, IService
{
    public CrowHolder crowHolder;

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









    //public void AddCrowTargetIfNoneExists(BirdseedController birdseedToFeastUpon)//Vector3 feast)
    //{

    //    //Instantiate(CrowTargetPrefab,feast, Quaternion.identity);

    //    if (!CrowTarget.GetComponent<CrowTarget>().isActiveTarget)
    //    {
    //        CrowTarget.transform.position = birdseedToFeastUpon.transform.position;
    //        CrowTarget.GetComponent<CrowTarget>().SetActiveTarget();
    //        ServiceLocator.Get<GameManager>().activeBirdseed = birdseedToFeastUpon;
    //    }


    //    //foreach (BirdBrain i in crows)
    //    //{
    //    //    i.stateMachine.Enter("CrowScatterState");
    //    //}
    //}
}
