using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtonSelectionHandler : MonoBehaviour//, IPointerEnterHandler
{
    public List<MenuButton> selectableButtons;
    public bool exclusive = true;
    public bool selectOnEnable = true;
    public bool clearLastSelectedOnDisable = true;

    private MenuButton lastSelected = null; // Only includes the lastSelected if it's part of the possibleSelectors, unlike the built-in one.

    public void SetSelectedGameObject(int _newSelectedIndex)
    {
        SetSelectedGameObject(selectableButtons[_newSelectedIndex].gameObject);
    }
    private void SetSelectedGameObject(GameObject newSelected)
    {
        EventSystem.current.SetSelectedGameObject(newSelected);
    }
    public void OnPotentialEntered(MenuButton menuButton)
    {
        if (!enabled) return;
        if (selectableButtons.Contains(menuButton))
            SetSelectedGameObject(menuButton.gameObject);
    }
    private void OnEnable()
    {
        foreach (MenuButton selectableButton in selectableButtons)
        {
            selectableButton.menuButtonHighlightSelector = this;
        }
        if (selectOnEnable)
        {
            if (clearLastSelectedOnDisable || lastSelected == null)
                SetSelectedGameObject(selectableButtons[0].gameObject);
            else SetSelectedGameObject(lastSelected.gameObject);
        }
    }
    private void OnDisable()
    {
        if (clearLastSelectedOnDisable)
            lastSelected = null;
    }
    private void Update()
    {
        if (exclusive)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                SetLastSelectedOrFirstPossibleSelector();
            }
            else if (!selectableButtons.Contains(EventSystem.current.currentSelectedGameObject.GetComponent<MenuButton>()))
                    SetSelectedGameObject(selectableButtons[0].gameObject);
            
        }
        else
        {
            if (EventSystem.current.currentSelectedGameObject == null)
                SetLastSelectedOrFirstPossibleSelector();
        }

        if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.GetComponent<MenuButton>())
            lastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<MenuButton>();
    }

    private void SetLastSelectedOrFirstPossibleSelector()
    {
        if (lastSelected != null) SetSelectedGameObject(lastSelected.gameObject);
        else SetSelectedGameObject(selectableButtons[0].gameObject);
    }

    public void SetButtonsInteractability(bool _active)
    {
        foreach (MenuButton i in selectableButtons)
        {
            if (i.GetComponent<Button>() != null)
                i.GetComponent<Button>().interactable = _active;
        }
    }
}
