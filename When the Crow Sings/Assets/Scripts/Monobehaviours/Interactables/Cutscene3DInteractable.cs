using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cutscene3DInteractable : MonoBehaviour
{
    //This goes on the cutscene trigger

    public CutsceneFinder cutsceneFinder;
    public FishingRod fishingRod;
    public QTEInteractable qte;
    public MeshRenderer[] poleParts;
    public SkinnedMeshRenderer[] polePartsSkinned;
    //private bool playerInTrigger;
    private MeshRenderer[] fishingRodOnTheGround;
    public SpriteRenderer arrow;
    public ParticleSystem rippleEffect;
    public ParticleSystem ripple2;
    public ParticleSystem ripple3;
    public ParticleSystem splashEffect;
    public Animator animator;
    private LineRenderer lineRenderer;

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
        splashEffect.Stop();
        rippleEffect.Stop();
        ripple2.Stop();
        ripple3.Stop();
        lineRenderer.enabled = false;
    }
    
    public void SetPositionRotation(Vector3 targetCoordinates)
    {
        Transform playerTransform = fishingRod.GetComponentInParent<PlayerController>().gameObject.transform;
        animator = playerTransform.GetComponentInChildren<Animator>();
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
        animator.SetBool("animIsFishing", true); 
        qte.EmitStartQteSignal();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrow.enabled = true;
            //playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arrow.enabled = false;
            //playerInTrigger = false;
        }
    }

    private IEnumerator fadeDelay(bool fishingPoleParts)
    {
        yield return new WaitForSeconds(1.5f);

        fishingRod = FindObjectOfType<FishingRod>();
        poleParts = fishingRod.GetComponentsInChildren<MeshRenderer>();
        fishingRodOnTheGround = this.gameObject.GetComponentsInChildren<MeshRenderer>();
        lineRenderer = FindObjectOfType<LineRenderer>();

        lineRenderer.enabled = fishingPoleParts;

        foreach (MeshRenderer part in poleParts)
        {
            part.enabled = fishingPoleParts;
            Debug.Log("Setting the fishing rod in hand active");
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
            rippleEffect.Play();
        }

        animator.SetBool("animIsFishing", false);
        cutsceneFinder.fadeOutOfBlack();
    }

}
