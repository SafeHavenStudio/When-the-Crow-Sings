using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTEInteractable : MonoBehaviour
{
    public QuickTimeEvent qte;

    [Header("Built-In")]
    public GameSignal globalQteSignal;

    public SerializableDictionary successDict;
    public SerializableDictionary failureDict;
    public GameSignal successSignal;
    public GameSignal failureSignal;
    public AudioClip successSound;
    public AudioClip failSound;
    public AudioSource audioSource;

    //This is for the qte script to use as a sound check to see which sound to play
    /*public enum QTETYPES
    {
        isSoup,
        isFishing,
        isFaridaMeter,
        isNone
    }

    public QTETYPES type = QTETYPES.isSoup;*/

    private SignalArguments signalArgs;
    private void Awake()
    {
        signalArgs = new SignalArguments();
        signalArgs.objectArgs.Add(qte);
        audioSource = GetComponent<AudioSource>();

        //if (successDict.elements.Count == 0) throw new System.Exception("QTE does not have success dict!");
        //if (failureDict.elements.Count == 0) throw new System.Exception("QTE does not have failure dict!");
        //successDict.ValidateNotBlank();
        //failureDict.ValidateNotBlank();

        if (successDict.elements.Count != 0)
        {
            successDict.ValidateNotBlank();
        }
        if (failureDict.elements.Count != 0)
        {
            failureDict.ValidateNotBlank();
        }

        SuccessAndFailValues successAndFailValues = new SuccessAndFailValues();
        foreach (SerializableDictionaryElement i in successDict.elements)
        {
            successAndFailValues.success[i.key] = i.value;
        }
        foreach (SerializableDictionaryElement i in failureDict.elements)
        {
            successAndFailValues.fail[i.key] = i.value;
        }
        signalArgs.objectArgs.Add(successAndFailValues);
        signalArgs.objectArgs.Add(successSignal);
        signalArgs.objectArgs.Add(failureSignal);
        Debug.Log("Objectargs is now " + signalArgs.objectArgs.Count.ToString());

        signalArgs.objectArgs.Add(this);
    }

    public void EmitStartQteSignal()
    {
        globalQteSignal.Emit(signalArgs);
    }

    public AudioSource GetAudioSource() { return audioSource; }

    //public GameObject visualCue;
    //public bool playerInRange;

    //void Awake()
    //{
    //    visualCue.SetActive(false);
    //    playerInRange = false;
    //}

    //public void ActivateTimingMeter()
    //{
    //    FindObjectOfType<TimingMeter>().startQTE();
    //}

    //private void OnTriggerEnter(Collider QTE)
    //{
    //    if (QTE.CompareTag("Player"))
    //    {
    //        visualCue.SetActive(true);
    //        playerInRange = true;
    //    }
    //}

    //private void OnTriggerExit(Collider QTE)
    //{
    //    if (QTE.CompareTag("Player"))
    //    {
    //        visualCue.SetActive(false);
    //        playerInRange = false;
    //    }
    //}
}
