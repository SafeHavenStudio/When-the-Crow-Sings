using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowRaycast : MonoBehaviour
{
    public float maxDistance = 1f;
    public LayerMask layerMask;


    Vector3 hitPoint = Vector3.zero;

    private void Update()
    {
        RaycastInFront();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLineList(new List<Vector3>() { transform.position, hitPoint }.ToArray());
    }

    void RaycastInFront()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit,maxDistance, layerMask))
        {
            hitPoint = hit.point;
        }
    }
}
