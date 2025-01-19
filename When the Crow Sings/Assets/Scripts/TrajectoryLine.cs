using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    public ThrowTarget throwTarget;
    LineRenderer lineRenderer;

    public float vertexCount = 10;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    //private void Update()
    //{
    //    for(int i = 0; i < lineRenderer.positionCount;  i++)
    //    {
    //        lineRenderer.SetPosition(i, transform.position+new Vector3 (0,1,0));
    //    }
    //    lineRenderer.SetPosition(0,transform.position);
    //    lineRenderer.SetPosition(2,throwTarget.transform.position);
    //}

    private void Update()
    {
        var pointList = new List<Vector3>();

        Vector3 point2 = new Vector3();
        point2 = (transform.position + throwTarget.transform.position) / 2;
        point2.y += 3;

        for(float ratio = 0; ratio <= 1; ratio += 1 / vertexCount)
        {
            var tangent1 = Vector3.Lerp(transform.position, point2, ratio);
            var tangent2 = Vector3.Lerp(point2, throwTarget.transform.position, ratio);
            var curve = Vector3.Lerp(tangent1,tangent2,ratio);

            pointList.Add(curve);
        }

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.SetPositions(pointList.ToArray());
    }
}
