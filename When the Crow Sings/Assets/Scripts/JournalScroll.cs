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
        StartCoroutine(ResetScrollbarAfterOneFrameBecauseUnityIsDumb());
    }
    private void OnDisable()
    {
        InputManager.playerInputActions.UI.Journal_Scroll.performed -= OnJournalScrollPerformed;
    }

    IEnumerator ResetScrollbarAfterOneFrameBecauseUnityIsDumb()
    {
        yield return null;
        scrollbar.value = 1.0f;
    }

    void OnJournalScrollPerformed(InputAction.CallbackContext context)
    {
        
        if (context.ReadValue<float>() != 0f)
            scrollbar.value = Mathf.Clamp01(scrollbar.value + Mathf.Sign(context.ReadValue<float>())*speedMult*scrollbar.size);
        Debug.Log("Scroll scroll scroll your boat..." + scrollbar.value.ToString());
    }
}
