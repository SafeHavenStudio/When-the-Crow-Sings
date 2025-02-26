using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThreeDCutscene : MonoBehaviour
{
    //This goes on the cutscene trigger

    public CutsceneFinder cutsceneFinder;
    public StartFishing startFishing;
    private QTEInteractable qte;
    public MeshRenderer[] poleParts;
    private bool playerInTrigger;
    bool started = false;

    private void Start()
    {
        startFishing = FindObjectOfType<StartFishing>();
        qte = GetComponentInChildren<QTEInteractable>();
    }

    private void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E) && !started)
        {
            started = true;
            cutsceneFinder.fadeToBlack();
            StartCoroutine(fadeDelay());
            StartCoroutine(fishCatchTimer());
        }
    }

    public IEnumerator fishCatchTimer()
    {
        yield return new WaitForSeconds(Random.Range(5, 20));
        qte.EmitStartQteSignal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private IEnumerator fadeDelay()
    {
        yield return new WaitForSeconds(1.5f);

        poleParts = startFishing.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer part in poleParts)
        {
            part.enabled = true;
        }

        cutsceneFinder.fadeOutOfBlack();
    }
}
