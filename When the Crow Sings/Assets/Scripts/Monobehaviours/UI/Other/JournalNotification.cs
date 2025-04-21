using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalNotification : MonoBehaviour
{
    [Header("InitializationThings")]
    public string message = "MSG_Not_Found";

    public float timeToStay = 3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(disappearAfterDelay());
    }

    IEnumerator disappearAfterDelay()
    {
        yield return new WaitForSecondsRealtime(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.JournalNotif);
        yield return new WaitForSecondsRealtime(timeToStay);
        GetComponent<Animator>().SetTrigger("GoOut");
        yield return new WaitForSecondsRealtime(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
