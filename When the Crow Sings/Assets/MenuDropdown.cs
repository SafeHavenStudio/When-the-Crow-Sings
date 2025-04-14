using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuDropdown : MonoBehaviour
{
    public MenuButtonSelectionHandler previousMenusMBSH;

    public TextMeshProUGUI VisibleText;

    public GameObject dropdownPopupButtonTemplate;

    public Transform buttonsParent;

    List<Button> dropdownPopupButtons = new List<Button>();

    MenuDropdownButton _currentlySelectedButton;

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

    public void AddDropdownButton(string buttonText)
    {
        Button _newButton = Instantiate(dropdownPopupButtonTemplate).GetComponent<Button>();
        dropdownPopupButtons.Add(_newButton);
        _newButton.transform.SetParent(buttonsParent, false);
        _newButton.GetComponent<MenuDropdownButton>().SetButtonText(buttonText);
        _newButton.gameObject.SetActive(true);

        if (dropdownPopupButtons.Count > 1)
        {
            Button _previousButton = dropdownPopupButtons[dropdownPopupButtons.Count - 2];

            Navigation _newButtonNavigation = new Navigation();
            _newButtonNavigation.mode = Navigation.Mode.Explicit;
            _newButtonNavigation.selectOnUp = _previousButton;
            _newButtonNavigation.selectOnDown = dropdownPopupButtons[0];
            _newButton.GetComponent<Button>().navigation = _newButtonNavigation;

            Navigation _previousButtonUpdatedNavigation = new Navigation();
            _previousButtonUpdatedNavigation.mode = Navigation.Mode.Explicit;
            _previousButtonUpdatedNavigation.selectOnUp = _previousButton.navigation.selectOnUp;
            _previousButtonUpdatedNavigation.selectOnDown = _newButton;
            _previousButton.navigation = _previousButtonUpdatedNavigation;

            Navigation _firstButtonUpdatedNavigation = new Navigation();
            _firstButtonUpdatedNavigation.mode = Navigation.Mode.Explicit;
            _firstButtonUpdatedNavigation.selectOnDown = dropdownPopupButtons[0].navigation.selectOnDown;
            _firstButtonUpdatedNavigation.selectOnUp = _newButton;
            dropdownPopupButtons[0].navigation = _firstButtonUpdatedNavigation;
        }
        else
        {
            SetCurrentlySelectedButton(_newButton.GetComponent<MenuDropdownButton>());
        }

        GetComponent<MenuButtonSelectionHandler>().selectableButtons.Add(_newButton.GetComponent<MenuButton>());
    }

    public UnityEvent<int> DropdownMenuButtonPressed;
    public void OnDropdownButtonPressed(Button _pressedButton)
    {
        int _pressedButtonIndex = dropdownPopupButtons.IndexOf(_pressedButton);

        SetCurrentlySelectedButton(_pressedButton.GetComponent<MenuDropdownButton>());

        DropdownMenuButtonPressed.Invoke(_pressedButtonIndex);
        ClosePopup();
    }


    public void SetCurrentlySelectedButton(MenuDropdownButton _newSelection)
    {
        _currentlySelectedButton = _newSelection;
        VisibleText.text = _currentlySelectedButton.textName;
    }
    public void SetCurrentlySelectedButton(int _newSelectionIndex)
    {
        _currentlySelectedButton = dropdownPopupButtons[_newSelectionIndex].GetComponent<MenuDropdownButton>();
        VisibleText.text = _currentlySelectedButton.textName;
    }
}
