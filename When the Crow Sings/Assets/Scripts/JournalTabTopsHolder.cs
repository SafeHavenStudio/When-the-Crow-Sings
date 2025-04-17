using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalTabTopsHolder : MonoBehaviour
{
    public List<GameObject> objectsToCheckIfEnabled;

    public List<GameObject> tabs;

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

        SetTabs(objectIsEnabled);
    }

    void SetTabs(bool objectIsEnabled)
    {
        if (objectIsEnabled)
        {
            foreach (GameObject i in tabs)
            {
                i.SetActive(objectIsEnabled);
            }
        }
        else
        {
            if (!disableTabsIsRunning)
                StartCoroutine(DisableTabs());
        }
        
    }
    bool disableTabsIsRunning = false;
    IEnumerator DisableTabs()
    {
        disableTabsIsRunning = true;
        yield return new WaitForSecondsRealtime(.1f);
        foreach (GameObject i in tabs)
        {
            i.SetActive(false);
        }
        disableTabsIsRunning = false;
    }
}