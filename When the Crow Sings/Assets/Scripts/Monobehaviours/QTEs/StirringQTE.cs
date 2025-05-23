using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using FMOD.Studio;
using FMODUnity;

public class StirringQTE : QuickTimeEvent
{
    public GameObject displayBox;
    private int currentStep = 0;
    private int RightCurrentStep = 0;
    private bool correctKey;
    private bool countingDown;
    public float switchCount = 5;
    private int soundIndex = 13;
    [HideInInspector] public bool complete = false;
    public float score = 0;
    //public float timer = 8;
    private bool firstTime = true;
    [HideInInspector] public bool failed = false;
    private bool aboveZero = false; //checks if the qte was started and drops back to 0 if failed
    private Cutscene3DInteractable cutsceneInteractable;
    private int lastShownStep = -1;

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

    private EventInstance FishingReelSound;

    public enum QTETYPES
    {
        isSoup,
        isFishing,
        isFaridaMeter,
        isNone
    }

    public QTETYPES type = QTETYPES.isSoup;

    //private bool isControllerConnected;

    private void Awake()
    {
        isDecaying = GameSettings.GetModel().qteDecay;
    }

    private void Start()
    {
        if (type == QTETYPES.isFishing)
        {
            FishingReelSound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.FishingReel);
            cutsceneInteractable = FindObjectOfType<Cutscene3DInteractable>();
            updateMusic();
            cutsceneInteractable.splashEffect.Play();
            cutsceneInteractable.ripple2.Play();
            cutsceneInteractable.ripple3.Play();
        }

        StartQTEFr();
    }

    private void updateMusic()
    {
        PLAYBACK_STATE playbackState;
        FishingReelSound.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            FishingReelSound.start();
        }
        else
        {
            FishingReelSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    public void StartQTEFr()
    {
        StartCoroutine(randomizeInput());

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

        //Check for QTE completion
        if (score >= slider.maxValue && firstTime)
        {
            SucceedQTE();
            updateMusic();
        }

        //Checks if the qte was started and if it drops back to 0, fail, also start decaying
        if (score > 0)
        {
            aboveZero = true;

            if(isDecaying)
            StartCoroutine(decayDelay());
        }
        else if (score <= 0 && aboveZero == true)
        {
            aboveZero = false;
            FailQTE();
            updateMusic();
        }
    }

    private IEnumerator decayDelay()
    {
        yield return new WaitForSeconds(1.5f);
        score -= 0.01f;
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

        if (type == QTETYPES.isFishing)
        {
            cutsceneInteractable = FindObjectOfType<Cutscene3DInteractable>();
            cutsceneInteractable.FinishCutscene();
        }
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

        if (type == QTETYPES.isFishing)
        {
            cutsceneInteractable.FinishCutscene();
        }
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

        Image[] keyColor = { wKey, aKey, sKey, dKey};

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

        Image[] keyDirection = { upJoystick, rightJoystick, downJoystick, leftJoystick };

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

        if (Input.anyKeyDown)
        {
            KeyCode expectedKey = keySequence[currentStep];

            if (Input.GetKeyDown(expectedKey) && (type != QTETYPES.isFishing))
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
            else if (type == QTETYPES.isFishing)
            {
                if (Input.GetKey(rightKeyHold))
                {
                    if (Input.GetKeyDown(expectedKey))
                    {
                        correctKey = true;
                        KeyPressFeedback();
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
        if (RightCurrentStep == lastShownStep) return;

        lastShownStep = RightCurrentStep;

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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.WaterSplash);
            Debug.Log("Playing splash");

        }
    }

    public void showRightJoystickDirection()
    {
        if (RightCurrentStep == lastShownStep) return;

        lastShownStep = RightCurrentStep;

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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.WaterSplash);
            Debug.Log("Playing splash");
        }
    }

    private void CheckJoystickInput()
    {
        Vector2 expectedDirection = joystickSequence[currentStep];
        Vector2 joystickInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 rightJoystickInput = new Vector2(Input.GetAxis("RightJoystickHorizontal"), Input.GetAxis("RightJoystickVertical"));
        Vector2 rightJoystickHold = RightJoystickSequence[RightCurrentStep];

        bool isRightJoystickHeld = (Vector2.Dot(rightJoystickInput.normalized, rightJoystickHold) > inputThreshold) && (rightJoystickInput.magnitude > inputThreshold);

        // Normal case: check left joystick input only
        if (type != QTETYPES.isFishing)
        {
            if (Vector2.Dot(joystickInput.normalized, expectedDirection) > inputThreshold)
            {
                correctKey = true;
                KeyPressFeedback();

                if (type == QTETYPES.isSoup)
                {
                    soundIndex++;
                    if (soundIndex % 22 == 0) // Plays it every 22nd press
                        AudioManager.instance.PlayOneShot(FMODEvents.instance.Swirl);
                }
            }
        }
        else // Special case for fishing QTE
        {
            if (isRightJoystickHeld) // Right joystick must be held before checking left joystick input
            {
                if (Vector2.Dot(joystickInput.normalized, expectedDirection) > inputThreshold)
                {
                    correctKey = true;
                    KeyPressFeedback();
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



