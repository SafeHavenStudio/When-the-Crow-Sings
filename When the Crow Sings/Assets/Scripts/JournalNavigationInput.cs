using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JournalNavigationInput : MonoBehaviour
{
    public MenuSwapper menuSwapper;

    private void OnEnable()
    {
        InputManager.playerInputActions.UI.Journal_ChangeLeft.performed += OnChangeLeft;
        InputManager.playerInputActions.UI.Journal_ChangeRight.performed += OnChangeRight;
    }

    private void OnDisable()
    {
        InputManager.playerInputActions.UI.Journal_ChangeLeft.performed -= OnChangeLeft;
        InputManager.playerInputActions.UI.Journal_ChangeRight.performed -= OnChangeRight;
    }

    void OnChangeLeft(InputAction.CallbackContext context)
    {
        menuSwapper.OpenPreviousMenu();
    }
    void OnChangeRight(InputAction.CallbackContext context)
    {
        menuSwapper.OpenNextMenu();
    }
}
