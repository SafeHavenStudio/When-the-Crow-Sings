using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{
    //[SerializeField]
    //public MenuButtonSelectionHandler menuButtonSelectionHandlerToSuspendWhileActive;


    //GameObject _dropdownList;
    //protected override GameObject CreateDropdownList(GameObject template)
    //{
    //    menuButtonSelectionHandlerToSuspendWhileActive.enabled = false;

    //    _dropdownList = base.CreateDropdownList(template);
    //    return _dropdownList;
    //}

    //protected override DropdownItem CreateItem(DropdownItem itemTemplate)
    //{
    //    DropdownItem _dropdownItem = base.CreateItem(itemTemplate);
    //    _dropdownList.GetComponent<MenuButtonSelectionHandler>().selectableButtons.Add(itemTemplate.GetComponent<MenuButton>());
    //    return _dropdownItem;
    //}

    //protected override void DestroyDropdownList(GameObject dropdownList)
    //{
    //    menuButtonSelectionHandlerToSuspendWhileActive.enabled = true;
    //    _dropdownList = null;
    //    base.DestroyDropdownList(dropdownList);
    //}
}
