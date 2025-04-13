using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDropdown : MonoBehaviour
{
    public MenuButtonSelectionHandler previousMenusMBSH;

    public void ClosePopup()
    {
        previousMenusMBSH.enabled = true;
        gameObject.SetActive(false);
    }

    public void OpenPopup()
    {
        gameObject.SetActive(true);
        previousMenusMBSH.enabled = false;
    }
}
