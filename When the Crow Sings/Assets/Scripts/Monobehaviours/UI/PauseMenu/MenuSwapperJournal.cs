using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwapperJournal : MenuSwapper
{
    public PageFlipper pageFlipper;
    //protected override void AdditionalMenuLogic(int whichMenu)
    //{
    //    // TODO: try to figure out the easiest way to put this in a child class maybe so it's not clogging up everything else...
    //}

    public override void OpenMenu(int whichMenu)
    {
        int currentLoop = 0;
        currentMenuIndex = whichMenu;
        playSound.Invoke();
        foreach (var menu in menus)
        {
            if (currentLoop == whichMenu)
                menu.SetActive(true);
            else menu.SetActive(false);

            //AdditionalMenuLogic(whichMenu);

            currentLoop++;
        }
    }
}
