using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataToAnimator : MonoBehaviour
{
    [SerializeField] Animator animator;

    //Dictionary<string,string> saveDataToAnimationMap = new Dictionary<string, string>() {
    //{ "b","a" },


    //};

    [SerializeField] List<string> stringsToAnimations = new List<string>();

    // Update is called once per frame
    void Update()
    {
        foreach (string i in stringsToAnimations)
        {
            animator.SetBool(i, SaveDataAccess.saveData.boolFlags[i]);
        }
    }
}
