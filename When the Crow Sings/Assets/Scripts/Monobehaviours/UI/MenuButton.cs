using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
    [HideInInspector]
    public MenuButtonSelectionHandler menuButtonHighlightSelector;
    public void OnPointerEnter(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        menuButtonHighlightSelector.OnPotentialEntered(this);
    }

    public void PressButton()
    {
        if (GetComponent<Button>().interactable)
        {
            Debug.Log("MenuButton.PressButton() called for " + gameObject.name + "!");
            ExecuteEvents.Execute(gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }
}
