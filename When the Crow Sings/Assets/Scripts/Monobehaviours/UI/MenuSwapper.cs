using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuSwapper : MonoBehaviour
{
    public List<GameObject> menus;

    public int currentMenuIndex = -1;

    public UnityEvent playSound;

    public void OpenMenu(int whichMenu)
    {
        int currentLoop = 0;
        currentMenuIndex = whichMenu;
        playSound.Invoke();
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
        if (_newIndex >= menus.Count) _newIndex = menus.Count - 1;
        OpenMenu(_newIndex);
    }
    public void OpenPreviousMenu()
    {
        int _newIndex = currentMenuIndex - 1;
        if (_newIndex < 0) _newIndex = 0;
        OpenMenu(_newIndex);
    }
}
