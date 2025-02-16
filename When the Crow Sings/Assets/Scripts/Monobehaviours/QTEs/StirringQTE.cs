using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class StirringQTE : QuickTimeEvent
{

    public GameObject displayBox;
    private int currentStep = 0;
    private int RightCurrentStep = 0;
    private bool correctKey;
    private bool countingDown;
    public float switchCount;
    private int soundIndex = 13;
    [HideInInspector] public bool complete = false;
    public int score = 0;
    public float timer = 8;
    private bool firstTime = true;
    [HideInInspector] public bool failed = false;

    public Image upJoystick;
    public Image rightJoystick;
    public Image downJoystick;
    public Image leftJoystick;

    public Image rightUpJoystick;
    public Image rightRightJoystick;
    public Image rightDownJoystick;
    public Image rightLeftJoystick;

    public Image wKey;
    public Image aKey;
    public Image sKey;
    public Image dKey;

    public Image upArrow;
    public Image leftArrow;
    public Image downArrow;
    public Image rightArrow;

    private KeyCode[] keySequence = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
    private KeyCode[] RightKeySequence = { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
    private Vector2[] joystickSequence = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private Vector2[] RightJoystickSequence = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    private float inputThreshold = 0.8f; //Threshold for recognizing a joystick direction

    public QTEInteractable qteInteractable;
    public Slider slider;

    //private bool isControllerConnected;

    private void Start()
    {
        StartCoroutine(randomizeInput());
    }

    private void Update()
    {
        qteInteractable = FindObjectOfType<QTEInteractable>();
        slider = GetComponentInChildren<Slider>();
        if (complete == true)
        {
            Debug.Log("Complete is true");
        }

        showArrowDirection();

        if (!countingDown)
        {
            if (InputManager.IsControllerConnected)
            {
                ShowCurrentDirection();
                CheckJoystickInput();
            }
            else
            {
                ShowCurrentKey();
                CheckKeyboardInput();
            }
        }

        // Check for QTE completion
        if(score >= slider.maxValue && firstTime)
        {
            SucceedQTE();
        }

        // Timer countdown logic
        if (!countingDown && timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateTimer(timer);
        }
        else if (timer <= 0 && firstTime)
        {
            Debug.Log("Time is up, QTE Failed");
            //AudioManager.instance.PlayOneShot(FMODEvents.instance.QteFailed, this.transform.position);
            failed = true;
            FailQTE();
        }
    }

    private IEnumerator waitBeforeCompletion()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.QteSucceeded, this.transform.position);
        firstTime = false;
        yield return new WaitForSeconds(1f);
        SignalArguments args = new SignalArguments();
        args.boolArgs.Add(true);
        globalFinishedQteSignal.Emit(args);
        Debug.Log("QTE COMPLETE");
        firstTime = true;
    }

    private IEnumerator waitBeforeFail()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.QteFailed, this.transform.position);
        firstTime = false;
        yield return new WaitForSeconds(1f);
        SignalArguments args = new SignalArguments();
        args.boolArgs.Add(false);
        globalFinishedQteSignal.Emit(args);
        firstTime = true;
    }

    private void UpdateTimer(float currentTime)
    {
        float time = Mathf.FloorToInt(currentTime % 60);
    }

    //keyboard
    private void ShowCurrentKey()
    {
        KeyCode currentKey = keySequence[currentStep];
        upJoystick.enabled = false;
        rightJoystick.enabled = false;
        downJoystick.enabled = false;
        leftJoystick.enabled = false;

        if (currentStep == 0)
        {
            wKey.color = new Color(wKey.color.r, wKey.color.g, wKey.color.b, 1);
            aKey.color = new Color(aKey.color.r, aKey.color.g, aKey.color.b, .2f);
            sKey.color = new Color(sKey.color.r, sKey.color.g, sKey.color.b, .2f);
            dKey.color = new Color(dKey.color.r, dKey.color.g, dKey.color.b, .2f);
        }
        else if (currentStep == 1)
        {
            wKey.color = new Color(wKey.color.r, wKey.color.g, wKey.color.b, .2f);
            aKey.color = new Color(aKey.color.r, aKey.color.g, aKey.color.b, 1);
            sKey.color = new Color(sKey.color.r, sKey.color.g, sKey.color.b, .2f);
            dKey.color = new Color(dKey.color.r, dKey.color.g, dKey.color.b, .2f);
        }
        else if (currentStep == 2)
        {
            wKey.color = new Color(wKey.color.r, wKey.color.g, wKey.color.b, .2f);
            aKey.color = new Color(aKey.color.r, aKey.color.g, aKey.color.b, .2f);
            sKey.color = new Color(sKey.color.r, sKey.color.g, sKey.color.b, 1);
            dKey.color = new Color(dKey.color.r, dKey.color.g, dKey.color.b, .2f);
        }
        else if (currentStep == 3)
        {
            wKey.color = new Color(wKey.color.r, wKey.color.g, wKey.color.b, .1f);
            aKey.color = new Color(aKey.color.r, aKey.color.g, aKey.color.b, .1f);
            sKey.color = new Color(sKey.color.r, sKey.color.g, sKey.color.b, .1f);
            dKey.color = new Color(dKey.color.r, dKey.color.g, dKey.color.b, 1);
        }
    }

    //controller
    private void ShowCurrentDirection()
    {
        wKey.enabled = false;
        aKey.enabled = false;
        sKey.enabled = false;
        dKey.enabled = false;

        if (currentStep == 0)
        {
            upJoystick.enabled = true;
            rightJoystick.enabled = false;
            downJoystick.enabled = false;
            leftJoystick.enabled = false;
        }
        else if (currentStep == 1)
        {
            upJoystick.enabled = false;
            rightJoystick.enabled = true;
            downJoystick.enabled = false;
            leftJoystick.enabled = false;
        }
        else if (currentStep == 2)
        {
            upJoystick.enabled = false;
            rightJoystick.enabled = false;
            downJoystick.enabled = true;
            leftJoystick.enabled = false;
        }
        else if (currentStep == 3)
        {
            upJoystick.enabled = false;
            rightJoystick.enabled = false;
            downJoystick.enabled = false;
            leftJoystick.enabled = true;
        }

        //string direction = joystickSequence[currentStep].ToString();
        //displayBox.GetComponentInChildren<TextMeshProUGUI>().text = direction;
    }

    private void CheckKeyboardInput()
    {
        KeyCode rightKeyHold = RightKeySequence[RightCurrentStep];

        if (Input.GetKey(rightKeyHold))
        {
            //Debug.Log("Right key hold = " + rightKeyHold);
            if (Input.anyKeyDown)
            {
                KeyCode expectedKey = keySequence[currentStep];

                if (Input.GetKeyDown(expectedKey))
                {
                    correctKey = true;
                    KeyPressFeedback();
                    if (qteInteractable.isSoup)
                    {
                        soundIndex++;
                        if (soundIndex % 22 == 0) //Plays it every 22nd press
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                    }
                }
                else
                {
                    correctKey = false;
                    KeyPressFeedback();
                }
            }
        }
    }

    private IEnumerator randomizeInput()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            RightCurrentStep = Random.Range(0, 4);

            Debug.Log("Right currentStep = " + RightCurrentStep);
        }
    }

    public void showArrowDirection()
    {
        rightUpJoystick.enabled = false;
        rightRightJoystick.enabled = false;
        rightLeftJoystick.enabled = false;
        rightDownJoystick.enabled = false;

        if (RightCurrentStep == 0)
        {
            upArrow.enabled = true;
            rightArrow.enabled = false;
            leftArrow.enabled = false;
            downArrow.enabled = false;
        }
        else if (RightCurrentStep == 1)
        {
            upArrow.enabled = false;
            rightArrow.enabled = false;
            leftArrow.enabled = true;
            downArrow.enabled = false;
        }
        else if (RightCurrentStep == 2)
        {
            upArrow.enabled = false;
            rightArrow.enabled = false;
            leftArrow.enabled = false;
            downArrow.enabled = true;
        }
        else if (RightCurrentStep == 3)
        {
            upArrow.enabled = false;
            rightArrow.enabled = true;
            leftArrow.enabled = false;
            downArrow.enabled = false;
        }
    }

    public void showRightJoystickDirection()
    {
        if (InputManager.IsControllerConnected)
        {
            upArrow.enabled = false;
            rightArrow.enabled = false;
            leftArrow.enabled = false;
            downArrow.enabled = false;

            if (RightCurrentStep == 0)
            {
                rightUpJoystick.enabled = true;
                rightRightJoystick.enabled = false;
                rightLeftJoystick.enabled = false;
                rightDownJoystick.enabled = false;
            }
            else if (RightCurrentStep == 0)
            {
                rightUpJoystick.enabled = false;
                rightRightJoystick.enabled = false;
                rightLeftJoystick.enabled = true;
                rightDownJoystick.enabled = false;
            }
            else if (RightCurrentStep == 0)
            {
                rightUpJoystick.enabled = false;
                rightRightJoystick.enabled = false;
                rightLeftJoystick.enabled = false;
                rightDownJoystick.enabled = true;
            }
            else if (RightCurrentStep == 0)
            {
                rightUpJoystick.enabled = false;
                rightRightJoystick.enabled = true;
                rightLeftJoystick.enabled = false;
                rightDownJoystick.enabled = false;
            }
        }
    }

    private void CheckJoystickInput()
    {
        Vector2 expectedDirection = joystickSequence[currentStep];
        Vector2 joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (Vector2.Dot(joystickInput.normalized, expectedDirection) > inputThreshold)
        {
            correctKey = true;
            KeyPressFeedback();

            if (qteInteractable.isSoup)
            {
                soundIndex++;
                if(soundIndex % 22 == 0) //plays it every 22nd press
                AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
            }
        }
    }

    private void KeyPressFeedback()
    {
        countingDown = true;

        //if(correctKey) 
            //displayBox.GetComponent<Image>().color = Color.green;

        //yield return new WaitForSeconds(0.03f); //Time between transitions (less = faster)

        //displayBox.GetComponent<Image>().color = Color.white;

        if (correctKey)
        {
            currentStep = (currentStep + 1) % keySequence.Length;
            score++;
            timer = 7; //Reset timer
        }
        else
        {
            //score = Mathf.Max(0, score - 1);
        }

        correctKey = false;
        countingDown = false; //Ready for next input
    }

    public override void SucceedQTE()
    {
        complete = true;
        StartCoroutine(waitBeforeCompletion());
    }

    public override void FailQTE()
    {
        StartCoroutine(waitBeforeFail());
    }

    public override void StartQTE()
    {
        throw new System.NotImplementedException();
        //just kinda have this in here cuz quicktimeevent was getting mad at me :(
    }
}



