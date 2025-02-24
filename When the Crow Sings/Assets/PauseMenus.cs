using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PauseMenus : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }
}
