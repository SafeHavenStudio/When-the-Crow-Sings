using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowHolder : MonoBehaviour
{
    List<BirdBrain> crows = new List<BirdBrain>();
    public GameObject CrowPrefab;

    public void DestroyCrows()
    {
        foreach (BirdBrain i in crows)
        {
            Destroy(i.gameObject);
        }
        crows.Clear();
    }
    public void SpawnCrows(List<CrowRestPoint> _crowRestPoints)
    {
        if (_crowRestPoints.Count > 0)
        {
            foreach (CrowRestPoint i in _crowRestPoints)
            {
                addCrow(i);
            }
        }
    }

    private void addCrow(CrowRestPoint _crowRestPoint)
    {
        GameObject birdBrain = Instantiate(CrowPrefab, transform);
        crows.Add(birdBrain.GetComponent<BirdBrain>());
        birdBrain.GetComponent<BirdBrain>().crowHolder = this;
        birdBrain.GetComponent<BirdBrain>().SetRestPoint(_crowRestPoint);
    }
}
