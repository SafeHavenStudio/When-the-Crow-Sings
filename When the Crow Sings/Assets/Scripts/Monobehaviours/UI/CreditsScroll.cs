using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreditsScroll : MonoBehaviour
{
    public Vector3 scrollSpeed = new Vector3(0, .75f, 0);

    // Update is called once per frame
    void Update()
    {
        this.transform.position += scrollSpeed;
    }
}
