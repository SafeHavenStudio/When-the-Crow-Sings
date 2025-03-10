using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVirtualCameraTarget : MonoBehaviour
{
    public Transform target;

    private void LateUpdate()
    {
        transform.position = target.position;
    }
}
