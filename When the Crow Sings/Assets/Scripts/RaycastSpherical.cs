using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSpherical : MonoBehaviour
{
    public float sphereRadius = 1f;
    public int pointCount = 1000;
    public float viewAngle = 60f;

    List<Vector3> spherePoints = new List<Vector3>();
    List<Vector3> collisionPoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        spherePoints = DistributePointsAcrossSphere();
    }

    private void FixedUpdate()
    {
        FindUnobstructedDirection(transform.forward);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        List<Vector3> drawablePoints = new List<Vector3>();
        for (int i = 0; i < spherePoints.Count; i++) { 
        drawablePoints.Add(transform.position);
        drawablePoints.Add(transform.TransformDirection(spherePoints[i]));
        }

        Gizmos.DrawLineList(drawablePoints.ToArray());
        //Gizmos.DrawLine(Vector3.zero, FindUnobstructedDirection(transform.forward));

        Gizmos.color = Color.red;
        Gizmos.DrawLineList(collisionPoints.ToArray());
    }

    

    Vector3 FindUnobstructedDirection(Vector3 targetDirection)
    {
        Vector3 bestDirection = targetDirection;
        Vector3 currentForward = transform.forward; // ?????
        float furthestUnobstructedDistance = 0f;
        RaycastHit hit;

        collisionPoints = new List<Vector3>();

        for (int i = 0; i < spherePoints.Count - 1; i++) // TODO: find what "i < BoidHelper.rayDirections.Length" should be to replace i < 1.
        {
            Vector3 direction = transform.TransformDirection(spherePoints[i]);
            //Vector3 direction = (-transform.position + spherePoints[i]);
            if (Physics.SphereCast(transform.position, 1f ,direction, out hit, sphereRadius))
            {
                if (hit.distance > furthestUnobstructedDistance)
                {
                    bestDirection = direction;
                    furthestUnobstructedDistance = hit.distance;

                    //collisionPoints.Add(new Vector3(direction.x,direction.y + .1f, direction.z)); // Because every other entry needs to be 0 for the line drawing to work
                    collisionPoints.Add(spherePoints[i]);
                    collisionPoints.Add(hit.point);
                }
            }
            else
            {
                //Debug.Log("Best direction: " + direction.ToString());
                return direction;
            }
        }

        //Debug.Log("Best direction: " + bestDirection.ToString());
        return bestDirection;
    }


    List<Vector3> DistributePointsAcrossSphere()
    {
        List<Vector3> points = new List<Vector3>();
        const float GoldenRatio = 1.61803f;
        float pow = .5f; // Same as the square root.
        for (int i = 0; i < pointCount; i++)
        {
            float t = i / (pointCount - 1f);
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = 2 * Mathf.PI * GoldenRatio * i;

            float angle = 2 * Mathf.PI * GoldenRatio * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            points.Add(new Vector3(x, y, z) + transform.position);// *sphereRadius);
        }

        return points;
    }
}
