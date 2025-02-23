using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowRestPoint : MonoBehaviour
{
    public GameObject debugVisible;
    public Transform approachPoint;

    public enum CrowTypes { REGULAR, TASK }
    public CrowTypes crowType = CrowTypes.REGULAR;
    private void OnEnable()
    {
        ServiceLocator.Get<CrowManager>().RegisterCrowRestPoint(this);
    }
    private void OnDisable()
    {
        ServiceLocator.Get<CrowManager>().UnregisterCrowRestPoint(this);
    }
    private void Awake()
    {
        debugVisible.SetActive(false);
    }
}
