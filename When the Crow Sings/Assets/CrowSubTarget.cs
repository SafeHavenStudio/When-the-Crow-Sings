using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowSubTarget : MonoBehaviour
{
    public Vector3 approachPoint = Vector3.zero;

    public LayerMask layerMask;

    public static float approachDistance = 5f;
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, approachDistance, layerMask))
        {
            approachPoint = hit.point;
            Debug.Log("Hit!");
        }
        else
        {
            approachPoint = transform.position + (transform.forward * approachDistance);
        }
    }
}
