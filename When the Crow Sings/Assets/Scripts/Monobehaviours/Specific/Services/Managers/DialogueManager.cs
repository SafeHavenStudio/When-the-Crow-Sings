using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEditor;
using FMODUnity;

public class DialogueManager : MonoBehaviour, IService
{
    private DialogueResource dialogueResource;

    [Header("Dialogue UI Elements")]
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private GameObject dialogueChoiceButtonsHolder;
    [SerializeField] private List<GameObject> dialogueChoiceButtons;
    [SerializeField] private GameObject nextButton;
    public Image npcImageUi;
    public Image playerImageUi;
    public Image nameBox;
    public DialoguePortraits dialoguePortraits;

    [Header("Signals")]
    public GameSignal startDialogueSignal;
    public GameSignal finishDialogueSignal;

    [Header("Settings")]
    public float textSpeed = .03f;
    public float secondsBetweenAudioPlays = .05f;
    public float pauseMultiplier = 10f;
    public float secondsToDelayChoiceInput = .1f;
    public List<GameSignal> signalsDialogueCanUse;

    DialogueChoiceBlock activeChoiceBlock = null;
    DialogueConditionBlock activeConditionBlock = null;

    #region StartMethods()
    private void Awake()
    {
        RegisterSelfAsService();
        dialogueUI.SetActive(false);

    }
    public void RegisterSelfAsService()
    {
        ServiceLocator.Register<DialogueManager>(this);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("textSpeed")) textSpeed = PlayerPrefs.GetFloat("textSpeed");
    }
    #endregion


    public void StartDialogue(SignalArguments signalArgs)
    {
        if (signalArgs.objectArgs[0] is DialogueResource)
        {
            dialogueResource = (DialogueResource)signalArgs.objectArgs[0];
        }
        else
        {
            throw new Exception("Error! The component emitting the signal does not have a DialogueResource as its first ObjectArgument.");
        }
        dialogueUI.SetActive(true);
        DisableChoiceButtons();

        DialogueParser parser = new DialogueParser(dialogueResource);
        DialogueTitle tempHolderForTheTargetIndex = dialogueResource.dialogueLines.OfType<DialogueTitle>().ToList().Find(x => x.titleName == signalArgs.stringArgs[0]); // TODO: Error if no title is found. Though maybe the built-in ones are clear enough.

        canSkip = false;
        ControlLineBehavior(tempHolderForTheTargetIndex.titleIndex, tempHolderForTheTargetIndex.tabCount);

    }

    public void EndDialogue()
    {
        choiceButtonsMBHS.enabled = false;
        nextButtonMBHS.enabled = false;
        dialogueUI.SetActive(false);
        finishDialogueSignal.Emit();
    }



    void ControlLineBehavior(int index, int previousLineTabCount)
    {
        canNextLine = false;
        currentLine = index;
        DialogueBase newLine = dialogueResource.dialogueLines[index];

        // Check if we need to skip to after a choice block.
        if (activeChoiceBlock != null && activeChoiceBlock.choiceHasBeenMade && newLine.tabCount <= activeChoiceBlock.choiceTabCount)
        {
            activeChoiceBlock.choiceHasBeenMade = false;
            ControlLineBehavior(activeChoiceBlock.endIndex, newLine.tabCount);
            return;
        }

        // Check if we need to skip to after a condition block.
        if (activeConditionBlock != null && activeConditionBlock.conditionHasBeenDecided && newLine.tabCount <= activeConditionBlock.conditionTabCount)
        {
            activeConditionBlock.conditionHasBeenDecided = false;
            //Debug.Log("Should be "+ ((DialogueResponse)dialogueResource.dialogueLines[activeConditionBlock.endIndex]).dialogue);
            ControlLineBehavior(activeConditionBlock.endIndex, newLine.tabCount);
            return;
        }


        if (newLine is DialogueResponse)
        {

            DialogueResponse newLine2 = (DialogueResponse)newLine;
            //Debug.Log(newLine2.dialogue);
            nameText.text = newLine2.characterName;
            if (nameText.text == "") nameBox.enabled = false;
            else nameBox.enabled = true;

            SetPortraits(newLine2);

            StartCoroutine(TypeText(dialogueText, newLine2.dialogue, index));
        }

        else if (newLine is DialogueGoto)
        {
            ResetChoiceBlocks();
            ResetConditionBlocks();

            DialogueGoto newLine2 = (DialogueGoto)newLine;
            if (newLine2.isEnd)
            {
                EndDialogue();
            }
            else
            {
                DialogueTitle tempHolderForTheTargetIndex = dialogueResource.dialogueLines.OfType<DialogueTitle>().ToList().Find(x => x.titleName == newLine2.gotoTitleName);
                //Debug.Log(newLine2.gotoTitleName + " so we're going to " + tempHolderForTheTargetIndex.titleIndex);
                ControlLineBehavior(tempHolderForTheTargetIndex.titleIndex, previousLineTabCount);
            }
        }

        else if (newLine is DialogueChoice)
        {
            EnableChoiceButtons();

            //activeChoiceBlock = null;
            foreach (DialogueTitleBlock i in dialogueResource.dialogueTitleBlocks)
            {
                foreach (DialogueChoiceBlock ii in i.dialogueChoiceBlocks)
                {
                    if (ii.dialogueChoices.Contains(newLine))
                    {
                        activeChoiceBlock = ii;
                        //Debug.Log("alasdhflaskgdjhklasdfh");
                        break;
                    }
                }
                //Debug.Log("Well, nothing in that title block.");
            }


            if (activeChoiceBlock == null) { throw new Exception("THE THING IS BLANK YOU SILLY GOOSE"); }

            SetChoiceButtons();

        }

        else if (newLine is DialogueCondition)
        {
            foreach (DialogueTitleBlock i in dialogueResource.dialogueTitleBlocks)
            {
                foreach (DialogueConditionBlock ii in i.dialogueConditionBlocks)
                {
                    if (ii.allConditions.Contains(newLine))
                    {
                        activeConditionBlock = ii;
                        break;
                    }
                }
            }

            if (activeConditionBlock == null) { throw new Exception("THE CONDITION BLOCK IS BLANK YOU SILLY DUCK"); }

            //Debug.Log("About to call DoConditionalDialogueLogic()");
            DoConditionalDialogueLogic();


        }

        else if (newLine is DialogueMutation)
        {
            DoMutationLogic((DialogueMutation)newLine);
            ControlLineBehavior(index + 1, previousLineTabCount);
        }

        else // In case of an EmptyLine
        {
            ControlLineBehavior(index + 1, previousLineTabCount);
        }

        
    }
    public MenuButtonSelectionHandler choiceButtonsMBHS;
    public MenuButtonSelectionHandler nextButtonMBHS;
    void EnableChoiceButtons()
    {
        nextButton.SetActive(false);
        dialogueChoiceButtonsHolder.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(dialogueChoiceButtons[0].gameObject);
        choiceButtonsMBHS.enabled = true;
        nextButtonMBHS.enabled = false;

        SetDefaultChancePortrait();
    }
    private void DisableChoiceButtons()
    {
        dialogueChoiceButtonsHolder.SetActive(false);
        playerImageUi.gameObject.SetActive(false);
        nextButton.SetActive(true);
        //EventSystem.current.SetSelectedGameObject(nextButton);
        choiceButtonsMBHS.enabled = false;
        nextButtonMBHS.enabled = true;
    }

    private void SetChoiceButtons()
    {
        foreach (GameObject i in dialogueChoiceButtons)
        {
            i.SetActive(false);
        }
        int loop = 0;
        foreach (DialogueChoice i in activeChoiceBlock.dialogueChoices)
        {
            dialogueChoiceButtons[loop].SetActive(true);
            dialogueChoiceButtons[loop].GetComponentInChildren<TextMeshProUGUI>().text = i.choiceText;
            dialogueChoiceButtons[loop].GetComponent<DialogueChoiceButton>().dialogueLineIndex = i.choiceIndex;
            dialogueChoiceButtons[loop].GetComponent<DialogueChoiceButton>().dialogueChoice = i;
            loop++;
        }
        StartCoroutine(delayChoiceInputActivation());
    }

    IEnumerator delayChoiceInputActivation()
    {
        foreach (GameObject i in dialogueChoiceButtons)
        {
            i.GetComponent<Button>().interactable = false;
        }
        yield return new WaitForSeconds(secondsToDelayChoiceInput);
        foreach (GameObject i in dialogueChoiceButtons)
        {
            i.GetComponent<Button>().interactable = true;
        }
    }

    private void ResetChoiceBlocks()
    {
        foreach (DialogueTitleBlock i in dialogueResource.dialogueTitleBlocks)
        {
            foreach (DialogueChoiceBlock ii in i.dialogueChoiceBlocks)
            {
                ii.choiceHasBeenMade = false;
            }
        }
    }
    private void ResetConditionBlocks()
    {
        foreach (DialogueTitleBlock i in dialogueResource.dialogueTitleBlocks)
        {
            foreach (DialogueConditionBlock ii in i.dialogueConditionBlocks)
            {
                ii.conditionHasBeenDecided = false;
            }
        }
    }



    bool canPlayAudio = true;
    IEnumerator TypeText(TextMeshProUGUI textMesh, string text, int index)
    {
        dialogueText.maxVisibleCharacters = 0;
        textMesh.text = text;

        isSkipping = false; // 100% necessary right here.

        canPlayAudio = true;

        while (textMesh.maxVisibleCharacters <= textMesh.text.Length)
        {
            if (isSkipping)
            {
                textMesh.maxVisibleCharacters = textMesh.text.Length + 1;
                break;
            }
            else
            {
                float pauseBetweenChars = textSpeed;
                int characterIndex = Mathf.Clamp(textMesh.maxVisibleCharacters - 1, 0, textMesh.text.Length);
                char character = textMesh.text[characterIndex];
                //char previousCharacter = 'x';
                //char nextCharacter = 'x';
                //if (characterIndex -1 >= 0) previousCharacter = textMesh.text[characterIndex-1];
                //if (characterIndex+1 <= textMesh.maxVisibleCharacters-1) nextCharacter = textMesh.text[characterIndex + 1];
                foreach (char i in ".!?")
                {
                    if (character == i)// && previousCharacter != i && nextCharacter != i)
                    {
                        pauseBetweenChars *= pauseMultiplier;
                        break;
                    }
                }
                yield return new WaitForSeconds(pauseBetweenChars); // TODO: Make it so isSkipping interrupts this.
                textMesh.maxVisibleCharacters += 1;

                if (canPlayAudio && textMesh.maxVisibleCharacters < textMesh.text.Length-1)
                {
                    switch (dialoguePortraits.combinedConditions)
                    {
                        case 1:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.ChanceBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Chance");
                            break;
                        case 2:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.TheodoreBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Theodore");
                            break;
                        case 4:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.PhilomenaBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Phil");
                            break;
                        case 8:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.FaridaBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Farida");
                            break;
                        case 16:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.AngelBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Angel");
                            break;
                        case 32:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.CalebBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Caleb");
                            break;
                        case 64:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.BeauBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Beau");
                            break;
                        case 128:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.QuinnBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Quinn");
                            break;
                        case 256:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.JazmyneBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Jaz");
                            break;
                        case 512:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.FranciscoBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Francisco");
                            break;
                        case 1024:
                            AudioManager.instance.PlayOneShot(FMODEvents.instance.YuleBlip);
                            StartCoroutine(DelayBeforeAudioCanPlay());
                            //Debug.Log("Yule");
                            break;
                        default:
                            break;
                    }
                }
            }

            canSkip = true;
        }

        isSkipping = false; // This may be redundant, may not be.
        canNextLine = true;
    }

    
    IEnumerator DelayBeforeAudioCanPlay()
    {
        canPlayAudio = false;
        yield return new WaitForSeconds(secondsBetweenAudioPlays);
        canPlayAudio = true;
    }

    private int currentLine;
    private bool canNextLine = false;
    private bool isSkipping = false;
    private bool canSkip = false;

    public bool isInDialogue
    {
        get { return dialogueUI.activeInHierarchy; }
    }
    private void Update()
    {
        //if (isInDialogue && EventSystem.current.currentSelectedGameObject == null)
        //{
        //    if (nextButton.activeInHierarchy)
        //    {
        //        EventSystem.current.SetSelectedGameObject(nextButton.gameObject);
        //    }
        //    else if (dialogueChoiceButtons[0].activeInHierarchy)
        //    {
        //        EventSystem.current.SetSelectedGameObject(dialogueChoiceButtons[0].gameObject);
        //    }
        //}
        if (isInDialogue)
        {
            if (nextButton.activeInHierarchy)
            {
                nextButtonMBHS.enabled = true;
                choiceButtonsMBHS.enabled = false;
            }
            else if (dialogueChoiceButtons[0].activeInHierarchy)
            {
                nextButtonMBHS.enabled = false;
                choiceButtonsMBHS.enabled = true;
            }
        }
    }

    public void OnNextLineButtonPressed()
    {
        if (canNextLine)
        {

            ControlLineBehavior(currentLine + 1, dialogueResource.dialogueLines[currentLine].tabCount);
        }
        else
        {
            if (canSkip) isSkipping = true;
        }
    }

    public void OnDialogueChoiceButtonClicked(DialogueChoiceButton choiceButton)
    {
        DisableChoiceButtons();
        activeChoiceBlock.choiceHasBeenMade = true;

        int nextLine = choiceButton.dialogueLineIndex + 1;
        int choiceTabCount = choiceButton.dialogueChoice.tabCount;


        ControlLineBehavior(nextLine, choiceTabCount);
    }

    

    bool Conditions(DialogueCondition i, ref int next_index)
    {
        if (i.dataType == DialogueCondition.DataType.BOOL)
        {
            Dictionary<string, bool> dictionaryToCheck = SaveDataAccess.saveData.boolFlags;
            bool result = false;
            if (i.logicType == DialogueCondition.LogicType.IF)
            {
                if (dictionaryToCheck[i.variableKeyString] == i.boolData)
                {
                    next_index = i.conditionIndex;
                    result = true;
                }
            }
            else
            {
                next_index = i.conditionIndex;
                result = true;
            }
            if (i.operatorType == DialogueCondition.OperatorType.NOT_EQUAL_TO) return !result;
            return result;
        }
        else if (i.dataType == DialogueCondition.DataType.STRING)
        {
            Dictionary<string, string> dictionaryToCheck = SaveDataAccess.saveData.stringFlags;
            bool result = false;
            if (i.logicType == DialogueCondition.LogicType.IF)
            {
                if (dictionaryToCheck[i.variableKeyString] == i.stringData)
                {
                    next_index = i.conditionIndex;
                    result = true;
                }
            }
            else
            {
                next_index = i.conditionIndex;
                result = true;
            }
            if (i.operatorType == DialogueCondition.OperatorType.NOT_EQUAL_TO) return !result;
            return result;
        }
        else if (i.dataType == DialogueCondition.DataType.INT)
        {
            Dictionary<string, int> dictionaryToCheck = SaveDataAccess.saveData.intFlags;
            bool result = false;
            if (i.logicType == DialogueCondition.LogicType.IF)
            {
                if (i.operatorType == DialogueCondition.OperatorType.EQUAL_TO)
                {
                    if (dictionaryToCheck[i.variableKeyString] == i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
                if (i.operatorType == DialogueCondition.OperatorType.GREATER_THAN)
                {
                    if (dictionaryToCheck[i.variableKeyString] > i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
                if (i.operatorType == DialogueCondition.OperatorType.GREATER_THAN_OR_EQUAL_TO)
                {
                    if (dictionaryToCheck[i.variableKeyString] >= i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
                if (i.operatorType == DialogueCondition.OperatorType.LESS_THAN)
                {
                    if (dictionaryToCheck[i.variableKeyString] < i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
                if (i.operatorType == DialogueCondition.OperatorType.LESS_THAN_OR_EQUAL_TO)
                {
                    if (dictionaryToCheck[i.variableKeyString] <= i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
                if (i.operatorType == DialogueCondition.OperatorType.NOT_EQUAL_TO)
                {
                    if (dictionaryToCheck[i.variableKeyString] != i.intData)
                    {
                        next_index = i.conditionIndex;
                        result = true;
                    }
                }
            }
            else
            {
                next_index = i.conditionIndex;
                result = true;
            }
            return result;
        }
        
        
        else // "else" for UNASSIGNED
        {
            bool result = false; 
            next_index = i.conditionIndex;
            result = true;

            if (i.operatorType == DialogueCondition.OperatorType.NOT_EQUAL_TO) return !result;
            return result;
        }
        //return false;
    }

    void DoConditionalDialogueLogic()
    {
        int next_index = -1;

        foreach (DialogueCondition i in activeConditionBlock.allConditions)
        {
            if (Conditions(i, ref next_index)) break;
        }

        activeConditionBlock.conditionHasBeenDecided = true;
        ControlLineBehavior(next_index+1, activeConditionBlock.ifStatement.tabCount);
    }


    void SetPortraits(DialogueResponse response)
    {
        //newLine2.characterName;
        //newLine2.characterEmotion;
        //string filePath = "Assets/Art/Sprites/"+response.characterName+"/"+response.characterName+"Sprite"+response.characterEmotion+".png";//Theodore (Character 9)/theodoreSpriteAnger.png";
        //Debug.Log(filePath);
        Image activeImageObject = null;
        if (response.characterName == "Chance")
        {
            npcImageUi.gameObject.SetActive(false);
            playerImageUi.gameObject.SetActive(true);
            activeImageObject = playerImageUi;
        }
        else
        {
            npcImageUi.gameObject.SetActive(true);
            playerImageUi.gameObject.SetActive(false);
            activeImageObject = npcImageUi;
        }
        activeImageObject.sprite = dialoguePortraits.GetPortrait(response.characterName, response.characterEmotion);
        if (activeImageObject.sprite == null)
        {
            activeImageObject.gameObject.SetActive(false);
        }

    }

    void SetDefaultChancePortrait()
    {
        playerImageUi.gameObject.SetActive(true);
        playerImageUi.sprite = dialoguePortraits.GetPortrait("Chance", Constants.EMOTIONS.DEFAULT);
    }

    void DoMutationLogic(DialogueMutation mutation)
    {
        switch (mutation.actionType)
        {
            case DialogueMutation.ActionType.SET:
                DoMutationSetMath(mutation);
                break;
            case DialogueMutation.ActionType.EMIT:
                GameSignal signalToEmit = signalsDialogueCanUse[mutation.intData];
                signalToEmit.Emit();
                break;
            case DialogueMutation.ActionType.CALL:
                if (mutation.stringData == "ExampleDialogueMethod()")
                {
                    ExampleDialogueMethod();
                }
                else if (mutation.stringData.Contains("ReloadScene("))
                {
                    int argument = Utilities.GetSingleIntFromString(mutation.stringData);
                    ReloadScene(argument);
                }
                else if (mutation.stringData == "SaveGameToDisk()")
                {
                    Debug.Log("Saved!");
                    SaveDataAccess.WriteDataToDisk();
                }
                else if (mutation.stringData == "EraseGameFromDisk()")
                {
                    Debug.Log("Erased!");
                    StartCoroutine(SaveDataAccess.EraseDataFromDisk());
                }
                else
                {
                    throw new Exception("Invalid method name!");
                }
                break;
            default:
                break;
        }
    }

    void DoMutationSetMath(DialogueMutation mutation)
    {
        switch (mutation.operatorType)
        {
            case DialogueMutation.OperatorType.EQUALS:
                switch (mutation.dataType)
                {
                    case DialogueMutation.DataType.STRING:
                        SaveDataAccess.SetFlag(mutation.actionKey, mutation.stringData);
                        break;
                    case DialogueMutation.DataType.INT:
                        SaveDataAccess.SetFlag(mutation.actionKey, mutation.intData);
                        break;
                    case DialogueMutation.DataType.BOOL:
                        SaveDataAccess.SetFlag(mutation.actionKey,mutation.boolData);
                        break;
                }
                break;
            case DialogueMutation.OperatorType.PLUS_EQUALS:
                if (mutation.dataType == DialogueMutation.DataType.INT)
                {
                    SaveDataAccess.SetFlag(mutation.actionKey, SaveDataAccess.saveData.intFlags[mutation.actionKey]+mutation.intData);
                }
                break;
            case DialogueMutation.OperatorType.MINUS_EQUALS:
                if (mutation.dataType == DialogueMutation.DataType.INT)
                {
                    SaveDataAccess.SetFlag(mutation.actionKey, SaveDataAccess.saveData.intFlags[mutation.actionKey] - mutation.intData);
                }
                break;
        }
        
    }


    void ExampleDialogueMethod()
    {
        Debug.Log("Dialogue called this method!");
    }

    void ReloadScene(int spawnIndex)
    {
        EndDialogue();

        ServiceLocator.Get<GameStateManager>().ReloadCurrentScene(spawnIndex);
    }

}
