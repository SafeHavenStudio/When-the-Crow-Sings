using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChanceIK : MonoBehaviour
{
    public Animator playerAnimator;
    public Transform playerLookAtTransform;

    void Start()
    {

    }

    void OnAnimatorIK()
    {
        playerAnimator.SetLookAtWeight(1.0f);
        playerAnimator.SetLookAtPosition(playerLookAtTransform.transform.position);
    }
}
