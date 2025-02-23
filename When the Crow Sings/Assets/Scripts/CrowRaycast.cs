using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowRaycast : MonoBehaviour
{
    public float maxDistance = 1f;
    public LayerMask layerMask;


    Vector3 hitPoint = Vector3.zero;

    private void FixedUpdate()
    {
        SendRaycast(transform.forward);
    }

    void SendRaycast(Vector3 raycastDirection)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, raycastDirection, out hit, maxDistance, layerMask))
        {
            hitPoint = hit.point;
        }
        else
        {
            hitPoint = transform.position + (transform.forward * maxDistance);
        }
    }


}
