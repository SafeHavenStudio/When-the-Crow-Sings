using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutscene3DInteractable : MonoBehaviour
{
    //This goes on the cutscene trigger

    public CutsceneFinder cutsceneFinder;
    public FishingRod fishingRod;
    private QTEInteractable qte;
    public MeshRenderer[] poleParts;
    private bool playerInTrigger;
    private MeshRenderer[] fishingRodOnTheGround;
    private SpriteRenderer arrow;

    private void Start()
    {
        qte = GetComponentInChildren<QTEInteractable>();
        arrow = GetComponentInChildren<SpriteRenderer>();
    }

    public void startCutscene()
    {
        cutsceneFinder.fadeToBlack();
        StartCoroutine(fadeDelay(true));
        StartCoroutine(fishCatchTimer());
    }

    public void FinishCutscene()
    {
        cutsceneFinder.fadeToBlack();
        StartCoroutine(fadeDelay(false));
    }
    
    public void SetPositionRotation(Vector3 targetCoordinates)
    {
        Transform playerTransform = fishingRod.GetComponentInParent<PlayerController>().gameObject.transform;
        CharacterController characterController = fishingRod.GetComponentInParent<CharacterController>();
        characterController.enabled = false;
        playerTransform.SetPositionAndRotation((targetCoordinates), (Quaternion.Euler(0, 32, 0)));
        characterController.enabled = true;
        Debug.Log(playerTransform.position);
        Debug.Log("Character controller reenabled");
    }

    public IEnumerator fishCatchTimer()
    {
        yield return new WaitForSeconds(Random.Range(4, 8));
        qte.EmitStartQteSignal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrow.enabled = true;
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrow.enabled = false;
            playerInTrigger = false;
        }
    }

    private IEnumerator fadeDelay(bool fishingPoleParts)
    {
        yield return new WaitForSeconds(1.5f);

        fishingRod = FindObjectOfType<FishingRod>();
        poleParts = fishingRod.GetComponentsInChildren<MeshRenderer>();
        fishingRodOnTheGround = this.gameObject.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer part in poleParts)
        {
            part.enabled = fishingPoleParts;
        }

        foreach (MeshRenderer part in fishingRodOnTheGround)
        {
            part.enabled = !fishingPoleParts;
            Debug.Log("Picked the rod up off the ground");
        }

        //Set player position and rotation
        if (fishingPoleParts == true)
        {
            SetPositionRotation(new Vector3(54.5f, -.5f, 342.5f));
            Debug.Log("Sending chance to fish");
        }


        cutsceneFinder.fadeOutOfBlack();
    }

}
