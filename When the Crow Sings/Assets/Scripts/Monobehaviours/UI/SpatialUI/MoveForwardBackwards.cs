using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForwardBackwards : MonoBehaviour
{
    private bool goingForward = false;
    public float speed = .5f;
    private Vector3 forwardLimit;
    private Vector3 backLimit;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    Vector3 backwards;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;

        forwardLimit = startPosition + transform.up * .2f;
        backLimit = startPosition - transform.up * .2f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spriteRenderer.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (goingForward)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            if (Vector3.Dot(transform.position - startPosition, transform.up) >= Vector3.Dot(forwardLimit - startPosition, transform.up))
            {
                goingForward = false;
            }
        }
        else
        {
            transform.position -= transform.up * speed * Time.deltaTime;
            if (Vector3.Dot(transform.position - startPosition, transform.up) <= Vector3.Dot(backLimit - startPosition, transform.up))
            {
                goingForward = true;
            }
        }

    }
}
