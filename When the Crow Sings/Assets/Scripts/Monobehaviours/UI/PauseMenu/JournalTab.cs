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

    public List<GameObject> associatedPages;

    public List<JournalTabSide> journalTabSides = new List<JournalTabSide>();

    public bool isActivated = false;

    bool isHovering = false;

    protected virtual void Update()
    {
        bool isPageActive = false;
        foreach (GameObject i in associatedPages)
        {
            if (i.activeInHierarchy)
            {
                isPageActive = true;
                break;
            }
        }
        if (!isHovering)
        {
            SetActivateTab(isPageActive);
        }
    }
    private void Awake()
    {
        image.sprite = deactivatedSprite;
    }

    public virtual void SetActivateTab(bool activated)
    {
        animator.SetBool("isActivated", activated);

        isActivated = activated;
    }
    public virtual void ActivateTab()
    {
        isHovering = true;
        Debug.Log("Ping!");
        image.sprite = activatedSprite;
        SetActivateTab(true);
    }

    public virtual void DeactivateTab()
    {
        isHovering = false;
        Debug.Log("!gniP");
        image.sprite = deactivatedSprite;
    }

    private void OnEnable()
    {
        image.sprite = deactivatedSprite;
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
}
