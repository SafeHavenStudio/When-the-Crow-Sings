using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchObject : MonoBehaviour
{
    public Transform endPoint; // Fixed end point
    public float objectThickness = 1f; // Uniform scale for X and Z

    void Update()
    {
        if (endPoint == null)
            return;

        //Get the vector from this object to the end point
        Vector3 direction = endPoint.position - transform.position;

        //Get the distance between start (this object) and end
        float distance = direction.magnitude;

        //Scale the object: Y is length, X/Z is thickness
        Vector3 newScale = new Vector3(objectThickness, distance * 0.5f, objectThickness);

        transform.localScale = newScale;
    }
}





