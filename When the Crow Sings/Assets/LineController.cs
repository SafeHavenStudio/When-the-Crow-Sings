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
        endPoint = FindObjectOfType<EndPoint>(); 

        //Set lineRenderer to always have 2 points (start & end)
        lineRenderer.positionCount = 2;
    }

    void Update()
    {
        //Ensure the line renderer updates the correct positions each frame
        lineRenderer.SetPosition(0, start.position); 
        lineRenderer.SetPosition(1, endPoint.transform.position);
    }
}

