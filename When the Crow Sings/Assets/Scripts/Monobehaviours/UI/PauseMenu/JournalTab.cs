using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class JournalTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator animator;
    public Sprite activatedSprite;
    public Sprite deactivatedSprite;
    public Image image;

    public List<JournalTabSide> journalTabSides = new List<JournalTabSide>();

    public bool isActivated = false;

    public virtual void SetActivateTab(bool activated)
    {
        animator.SetBool("isActivated", activated);

        isActivated = activated;
    }
    public virtual void ActivateTab()
    {
        Debug.Log("Ping!");
        image.sprite = activatedSprite;
        SetActivateTab(true);
    }

    public virtual void DeactivateTab()
    {
        Debug.Log("!gniP");
        image.sprite = deactivatedSprite;
        SetActivateTab(false);
    }

    private void OnEnable()
    {
        image.sprite = deactivatedSprite;
        //StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        SetActivateTab(!isActivated);
        StartCoroutine(wait());
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        ActivateTab();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        DeactivateTab();
    }
    public void OnButtonPressed()
    {
        foreach (JournalTabSide i in journalTabSides)
        {

        }
    }
}
