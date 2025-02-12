using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageFlip : MonoBehaviour
{
    float flipSpeed = 100;
    public bool isRight;
    public Transform flipPoint;
    float targetAngle;
    private Vector3 currentEulerAngles;
    static PageFlip currentFlippingPage = null;

    private void Update()
    {
        if (currentFlippingPage == this)
        {
            currentEulerAngles.y = Mathf.MoveTowards(currentEulerAngles.y, targetAngle, Time.deltaTime * flipSpeed);

            flipPoint.localEulerAngles = currentEulerAngles;

            if (Mathf.Abs(currentEulerAngles.y - targetAngle) < 0.01f)
            {
                currentEulerAngles.y = 0;
                flipPoint.localEulerAngles = Vector3.zero;
                //Debug.Log("Flip complete");
                currentFlippingPage = null;
                this.gameObject.SetActive(false);
            }
        }
    }

    public void flipLeft()
    {
        if(!isRight && currentFlippingPage == null)
        {
            this.gameObject.SetActive(true);
            currentFlippingPage = this;
            targetAngle = -180;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PageFlip);
            Debug.Log("Flipping left");
        }
    }

    public void flipRight()
    {
        if (isRight && currentFlippingPage == null)
        {
            this.gameObject.SetActive(true);
            currentFlippingPage = this;
            targetAngle = 180;
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PageFlip);
            Debug.Log("Flipping right");
        }
    }
}
