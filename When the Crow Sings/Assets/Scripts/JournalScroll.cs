using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JournalScroll : MonoBehaviour
{
    public Scrollbar scrollbar;
    public float speedMult = .1f;

    float currentDirection = 0f;

    private void OnEnable()
    {
        InputManager.playerInputActions.UI.Journal_Scroll.performed += OnJournalScrollPerformed;
        StartCoroutine(ResetScrollbarAfterOneFrameBecauseUnityIsDumb());
    }
    private void OnDisable()
    {
        InputManager.playerInputActions.UI.Journal_Scroll.performed -= OnJournalScrollPerformed;
        //InputManager.playerInputActions.UI.Journal_Scroll.IsPressed();
    }

    private void Update()
    {
        if (!InputManager.playerInputActions.UI.Journal_Scroll.IsPressed()) currentDirection = 0f;

        scrollbar.value = Mathf.Clamp01(scrollbar.value + (currentDirection * speedMult * scrollbar.size * Time.unscaledDeltaTime));
        Debug.Log("Current scroll direction is " + currentDirection.ToString() + " Scrolling to " + scrollbar.value.ToString());
    }

    IEnumerator ResetScrollbarAfterOneFrameBecauseUnityIsDumb()
    {
        yield return null;
        scrollbar.value = 1.0f;
    }

    void OnJournalScrollPerformed(InputAction.CallbackContext context)
    {

        //if (context.ReadValue<float>() != 0f)
        //    scrollbar.value = Mathf.Clamp01(scrollbar.value + Mathf.Sign(context.ReadValue<float>())*speedMult*scrollbar.size);
        //Debug.Log("Scroll scroll scroll your boat..." + scrollbar.value.ToString());
        if (context.ReadValue<float>() != 0f) currentDirection = Mathf.Sign(context.ReadValue<float>());
        else currentDirection = 0f;
    }
}
