using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform start; 
    public EndPoint endPoint; 

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        if (endPoint == null)
            endPoint = FindObjectOfType<EndPoint>();
        else return;

        //Set lineRenderer to always have 2 points (start & end)
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        SetLineRendererPosition();
    }

    void SetLineRendererPosition()
    {
        if (endPoint != null)
        {
            //Ensure the line renderer updates the correct positions each frame
            lineRenderer.SetPosition(0, start.position);
            lineRenderer.SetPosition(1, endPoint.transform.position);
        }
    }
}

