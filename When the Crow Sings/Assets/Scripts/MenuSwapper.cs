using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwapper : MonoBehaviour
{
    public List<GameObject> menus;

    public int currentMenuIndex = -1;

    public void OpenMenu(int whichMenu)
    {
        int currentLoop = 0;
        currentMenuIndex = whichMenu;
        foreach (var menu in menus)
        {
            if (currentLoop == whichMenu)
                menu.SetActive(true);
            else menu.SetActive(false);

            AdditionalMenuLogic(whichMenu);
            
            currentLoop++;
        }
    }

    protected virtual void AdditionalMenuLogic(int whichMenu) { return; }

    public void OpenNextMenu()
    {
        int _newIndex = currentMenuIndex + 1;
        if (_newIndex > menus.Count) _newIndex = menus.Count;
        OpenMenu(_newIndex);
    }
    public void OpenPreviousMenu()
    {
        int _newIndex = currentMenuIndex - 1;
        if (_newIndex < 0) _newIndex = 0;
        OpenMenu(_newIndex);
    }

    public void OpenMenu(GameObject whichMenu)
    {

    }
}
