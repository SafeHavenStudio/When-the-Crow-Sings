using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TimingMeterQTE : QuickTimeEvent
{
    public Slider sliderMeter;
    public int speed = 2;
    public float targetMin; //Range for successful hit
    public float targetMax;
    public Image background;
    //public float targetValue;
    public Animator timingMeterAnimator;

    public GameSettings gameSettings;

    public RectTransform targetMinMarker;  
    public RectTransform targetMaxMarker;
    public RectTransform targetRangeHighlight;
    public Image spacebar;
    public Image bKey;

    [HideInInspector]
    public int winCount;
    public int winCounter;

    private bool movingRight = true; //Meter movement direction
    public bool meterActive = false;
    private bool isFinished = false;

    private void Start()
    {
        if (InputManager.IsControllerConnected)
        {
            bKey.enabled = true;
            spacebar.enabled = false;
        }
        else
        {
            spacebar.enabled = true;
            bKey.enabled = false;
        }

        SetTargetRangeMarkers();
        RandomizeMeter();
        //leave out when implementation added
        StartQTE();
    }

    // Update is called once per frame
    void Update()
    {
        if (meterActive)
        {
            MoveMeter();
            //if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button1))
            //{
            //    CheckSuccess();
            //}
        }
    }

    public override void StartQTE()
    {
        Debug.Log("Started!");
        //timingMeterAnimator.SetBool("isOpen", true);
        meterActive = true;
        isFinished = false;
        SetTargetRangeMarkers();

        InputManager.playerInputActions.UI.QTEAccept.performed += OnQTEInput;
    }

    //Moves the handle left and right
    private void MoveMeter()
    {
        if (movingRight)
        {
            sliderMeter.value += Time.deltaTime * speed;
            if (sliderMeter.value >= 1f)
            {
                movingRight = false;
            }
        }
        else
        {
            sliderMeter.value -= Time.deltaTime * speed;
            if (sliderMeter.value <= 0f)
            {
                movingRight = true;
            }
        }
    }

    //Check if qte was successful

    private void OnQTEInput(InputAction.CallbackContext context)
    {
        CheckSuccess();
    }
    private void CheckSuccess()
    {
        //meterActive = false;

        if (sliderMeter.value >= targetMin && sliderMeter.value <= targetMax)
        {
            winCount++;
            StartCoroutine(waitForSuccess());
            Debug.Log(winCount + "/" + winCounter);
            if (winCount >= winCounter)
            {
                Debug.Log("Successful QTE");
                //RandomizeMeter();
                if(!isFinished)
                AudioManager.instance.PlayOneShot(FMODEvents.instance.QteSucceeded, this.transform.position);

                isFinished = true;
                gameSettings.speed = 0;
                background.color = Color.green;
                StartCoroutine(waitForSuccess());
                //SucceedQTE();
            }
            else if (winCounter > winCount)
            {
                Debug.Log("Else ifed");
                RandomizeMeter();
                SetTargetRangeMarkers();
                if(!isFinished)
                AudioManager.instance.PlayOneShot(FMODEvents.instance.QteSucceeded, this.transform.position);
            }
        }
        else
        {
            Debug.Log("Failed QTE");
            //SetTargetRangeMarkers();
            //RandomizeMeter();
            if(!isFinished)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.QteFailed, this.transform.position);
            isFinished = true;
            speed = 0;
            StartCoroutine(waitForFailure());
            //FailQTE();
        }
    }

    public override void SucceedQTE()
    {
        SignalArguments args = new SignalArguments();
        InputManager.playerInputActions.UI.QTEAccept.performed -= OnQTEInput;

        args.boolArgs.Add(true);
        globalFinishedQteSignal.Emit(args);
    }
    public override void FailQTE()
    {
        SignalArguments args = new SignalArguments();
        InputManager.playerInputActions.UI.QTEAccept.performed -= OnQTEInput;

        args.boolArgs.Add(false);
        globalFinishedQteSignal.Emit(args);
    }

    private IEnumerator waitForSuccess()
    {
        background.color = Color.green;

        yield return new WaitForSeconds(1f);

        if (winCount >= winCounter)
        SucceedQTE();
        else
        background.color = Color.white;
    }

    private IEnumerator waitForFailure()
    {
        background.color = Color.red;
        yield return new WaitForSeconds(1f);

        FailQTE();
    }

    public void RandomizeMeter()
    {
        /*targetValue = Random.Range(0.1f, 0.9f);
        targetMin = targetValue - 0.1f;
        targetMax = targetValue + 0.1f;*/

        //change these range values for accessibility settings
        targetMin = Random.Range(0.3f, 0.4f);
        targetMax = Random.Range(0.6f, 0.7f);
        meterActive = true;
        Debug.Log("Randomized");
        
        //EndQTE();
    }

    //Set the target markers based off target range
    public void SetTargetRangeMarkers()
    {
        //Get the width of the slider.
        float sliderWidth = sliderMeter.GetComponent<RectTransform>().rect.width;

        //Calculate the X positions of the markers based on the target range (0 to 1 range).
        float minXPos = sliderWidth * targetMin;
        float maxXPos = sliderWidth * targetMax;

        //Set the positions of the target markers relative to the fill area.
        targetMinMarker.anchoredPosition = new Vector2(minXPos, targetMinMarker.anchoredPosition.y);
        targetMaxMarker.anchoredPosition = new Vector2(maxXPos, targetMaxMarker.anchoredPosition.y);

        //Set the size and position of the highlight area
        float highlightWidth = maxXPos - minXPos;  //Width of the highlighted area
        targetRangeHighlight.sizeDelta = new Vector2(highlightWidth, targetRangeHighlight.sizeDelta.y); // Adjust width
        targetRangeHighlight.anchoredPosition = new Vector2(minXPos + 0.5f, targetRangeHighlight.anchoredPosition.y); // Adjust position
        //Debug.Log(targetRangeHighlight.sizeDelta + targetRangeHighlight.anchoredPosition);

        Debug.Log("Markers reset");
    }


}
