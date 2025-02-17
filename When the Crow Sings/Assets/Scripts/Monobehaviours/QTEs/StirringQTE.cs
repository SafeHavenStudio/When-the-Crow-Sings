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
    public float switchCount = 3;
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
    private Vector2[] RightJoystickSequence = { Vector2.up, Vector2.left, Vector2.down, Vector2.right };
    private float inputThreshold = 0.8f; //Threshold for recognizing a joystick direction

    //public QTEInteractable qteInteractable;
    public Slider slider;

    public enum QTETYPES
    {
        isSoup,
        isFishing,
        isFaridaMeter,
        isNone
    }

    public QTETYPES type = QTETYPES.isSoup;

    //private bool isControllerConnected;

    private void Start()
    {
        StartCoroutine(randomizeInput());

        //qteInteractable = FindObjectOfType<QTEInteractable>();

        slider = GetComponentInChildren<Slider>();

        if (type != QTETYPES.isFishing)
        {
            rightUpJoystick.enabled = false;
            rightRightJoystick.enabled = false;
            rightLeftJoystick.enabled = false;
            rightDownJoystick.enabled = false;
            upArrow.enabled = false;
            rightArrow.enabled = false;
            leftArrow.enabled = false;
            downArrow.enabled = false;
        }
    }

    private void Update()
    {
        if (!countingDown)
        {
            if (InputManager.IsControllerConnected)
            {
                if (type == QTETYPES.isFishing)
                    showRightJoystickDirection();

                ShowCurrentDirection();
                CheckJoystickInput();
            }
            else
            {
                if (type == QTETYPES.isFishing)
                    showArrowDirection();

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

        Image[] keyColor = { wKey, aKey, sKey, dKey };

        foreach (var key in keyColor)
        {
            key.color = new Color(key.color.r, key.color.g, key.color.b, .2f);
        }

        if (currentStep >= 0 && currentStep < keyColor.Length)
        {
            keyColor[currentStep].color = Color.white;
        }
    }

    //controller
    private void ShowCurrentDirection()
    {
        wKey.enabled = false;
        aKey.enabled = false;
        sKey.enabled = false;
        dKey.enabled = false;

        Image[] keyDirection = { wKey, aKey, sKey, dKey };

        foreach (var key in keyDirection)
        {
            key.enabled = false;
        }

        if (currentStep >= 0 && currentStep < keyDirection.Length)
        {
            keyDirection[currentStep].enabled = true;
        }
    }

    private void CheckKeyboardInput()
    {
        KeyCode rightKeyHold = RightKeySequence[RightCurrentStep];

            //Debug.Log("Right key hold = " + rightKeyHold);
        if (Input.anyKeyDown)
        {
            KeyCode expectedKey = keySequence[currentStep];

            if (Input.GetKeyDown(expectedKey) && (type != QTETYPES.isFishing))
            {
                correctKey = true;
                KeyPressFeedback();
                if (type == QTETYPES.isFishing)
                {
                    soundIndex++;
                    if (soundIndex % 22 == 0) //Plays it every 22nd press
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                }
            }
            else if (type == QTETYPES.isFishing)
            {
                if (Input.GetKey(rightKeyHold))
                {
                    if (Input.GetKeyDown(expectedKey))
                    {
                        correctKey = true;
                        KeyPressFeedback();
                        if (type == QTETYPES.isSoup)
                        {
                            soundIndex++;
                            if (soundIndex % 22 == 0) //Plays it every 22nd press
                                AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                        }
                    }
                }
            }
            else
            {
                correctKey = false;
                KeyPressFeedback();
            }
        }
    }

    private IEnumerator randomizeInput()
    {
        while (true)
        {
            yield return new WaitForSeconds(switchCount);
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

        Image[] arrowDirections = { upArrow, leftArrow, downArrow, rightArrow };

        foreach (var arrow in arrowDirections)
        {
            arrow.enabled = false;
        }

        if (RightCurrentStep >= 0 && RightCurrentStep < arrowDirections.Length)
        {
            arrowDirections[RightCurrentStep].enabled = true;
        }
    }

    public void showRightJoystickDirection()
    {
        upArrow.enabled = false;
        rightArrow.enabled = false;
        leftArrow.enabled = false;
        downArrow.enabled = false;

        //Store joystick UI elements in an array
        Image[] joystickDirections = { rightUpJoystick, rightLeftJoystick, rightDownJoystick, rightRightJoystick };

        //Disable all joystick indicators first
        foreach (var joystick in joystickDirections)
        {
            joystick.enabled = false;
        }

        //Enable the correct joystick indicator
        if (RightCurrentStep >= 0 && RightCurrentStep < joystickDirections.Length)
        {
            joystickDirections[RightCurrentStep].enabled = true;
        }
    }

        private void CheckJoystickInput()
    {
        Vector2 expectedDirection = joystickSequence[currentStep];
        Vector2 joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 rightJoystickInput = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical"));
        Vector2 rightJoystickHold = RightJoystickSequence[RightCurrentStep];

        //Debug.Log($"Right joystick input: {rightJoystickInput})");

        if (Vector2.Dot(joystickInput.normalized, expectedDirection) > inputThreshold)
        {
                correctKey = true;
                KeyPressFeedback();

                if (type == QTETYPES.isSoup)
            {
                    soundIndex++;
                    if (soundIndex % 22 == 0) //plays it every 22nd press
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                }
        }
        else if (type == QTETYPES.isFishing)
        {
            if (Vector2.Dot(rightJoystickInput.normalized, rightJoystickHold) > inputThreshold)
            {
                correctKey = true;
                KeyPressFeedback();

                if (type == QTETYPES.isSoup)
                {
                    soundIndex++;
                    if (soundIndex % 22 == 0) //plays it every 22nd press
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                }
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



