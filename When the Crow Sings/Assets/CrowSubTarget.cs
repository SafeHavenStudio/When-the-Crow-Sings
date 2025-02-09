using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowSubTarget : MonoBehaviour
{
    public Vector3 approachPoint = Vector3.zero;

    public LayerMask layerMask;

    public static float approachDistance = 5f;
    public Vector3 FindApproachPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, approachDistance, layerMask))
        {
            approachPoint = hit.point;
            approachPoint.y = transform.position.y + (transform.forward.y);
            Debug.Log("Hit!");
        }
        else
        {
            approachPoint = transform.position + (transform.forward * approachDistance);
        }
        return approachPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, approachPoint);
    }
}
