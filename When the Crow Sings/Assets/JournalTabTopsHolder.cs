using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTabTopsHolder : MonoBehaviour
{
    public List<GameObject> objectsToCheckIfEnabled;

    void Update()
    {
        bool objectIsEnabled = false;
        foreach (GameObject obj in objectsToCheckIfEnabled)
        {
            if (obj.activeInHierarchy)
            {
                objectIsEnabled = true;
                break;
            }
        }

        GetComponent<Animator>().SetBool("shouldBeVisible", objectIsEnabled);
        
    }
}