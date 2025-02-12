using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JournalScroll : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float speedMult = .1f;

    private void OnEnable()
    {
        InputManager.playerInputActions.UI.Journal_Scroll.performed += OnJournalScrollPerformed;
        scrollbar.value = 0;
    }
    private void OnDisable()
    {
        InputManager.playerInputActions.UI.Journal_Scroll.performed -= OnJournalScrollPerformed;
    }

    void OnJournalScrollPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Scroll scroll scroll your boat..." + context.ReadValue<float>().ToString());
        if (context.ReadValue<float>() != 0f)
            scrollbar.value = Mathf.Clamp01(scrollbar.value + Mathf.Sign(context.ReadValue<float>())*speedMult*scrollbar.size);
    }
}
