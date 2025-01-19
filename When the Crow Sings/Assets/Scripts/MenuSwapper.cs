using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwapper : MonoBehaviour
{
    public List<GameObject> menus;
    public GameObject journalHolder;

    public void OpenMenu(int whichMenu)
    {
        int currentLoop = 0;
        foreach (var menu in menus)
        {
            if (currentLoop == whichMenu)
                menu.SetActive(true);
            else menu.SetActive(false);
            if (whichMenu > 1) journalHolder.SetActive(true);
            else journalHolder.SetActive(false);
            currentLoop++;
        }
    }
    public void OpenMenu(GameObject whichMenu)
    {

    }
}
