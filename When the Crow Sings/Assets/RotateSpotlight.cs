using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class RotateSpotlight : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localRotation = Quaternion.Euler(180, 0, transform.localRotation.eulerAngles.z + (10 * Time.deltaTime));
        transform.Rotate(new Vector3(0,0,100 * Time.deltaTime));
    }
}
