using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public partial class DialogueParser
{
    private DialogueResource dialogueResource;
    string[] raw_lines;
    string rawText;
    string trimmedLine;

    // TODO: Figure out how to sort things so "blocks" can be structured.
    DialogueTitle currentTitle = null;
    DialogueTitleBlock currentTitleBlock = null;

    DialogueChoiceBlock currentChoiceBlock = null;
    DialogueConditionBlock currentConditionBlock = null;

    public DialogueParser(DialogueResource targetDialogueResource)
    {
        dialogueResource = targetDialogueResource;

        if (dialogueResource.dialogueLines.Count == 0)
        {
            PrepareDialogueResource(File.ReadAllText(dialogueResource.path));
        }
        
    }




    void PrepareDialogueResource(string text) // 
    {
        // Identify all DialogueBase line types and add them to dialogueResource.dialogueLines.
        IdentifyAndAssignDialogueLines(text);

        int current_loop = -1;
        foreach (DialogueBase i in dialogueResource.dialogueLines)
        {
            current_loop++;

            // Check if indentation indicates the current choice block has ended.
            if (currentChoiceBlock != null &&
                i.tabCount <= currentChoiceBlock.choiceTabCount && i is not DialogueChoice)
            {
                currentChoiceBlock.endIndex = current_loop;
                currentChoiceBlock = null;
            }

            // Start a new block for every title we encounter.
            if (i is DialogueTitle)
            {
                currentTitle = (DialogueTitle)i;
                currentTitleBlock = new DialogueTitleBlock(currentTitle);
                dialogueResource.dialogueTitleBlocks.Add(currentTitleBlock);
                continue;
            }
            if (dialogueResource.dialogueTitleBlocks.Count == 0) continue;
            

            // Since this line is not a title, the current line to the TitleBlock.
            if (currentTitleBlock == null) throw new System.Exception("Uh wait this shouldn't be possible hold up.");
            currentTitleBlock.dialogueLines.Add(i);


            // Organize the choice blocks.
            if (currentChoiceBlock == null)
            {
                if (i is DialogueChoice)
                {
                    DialogueChoice _i = (DialogueChoice)i;

                    currentChoiceBlock = new DialogueChoiceBlock();
                    currentTitleBlock.dialogueChoiceBlocks.Add(currentChoiceBlock);

                    currentChoiceBlock.choiceTabCount = _i.tabCount;

                    currentChoiceBlock.dialogueChoices.Insert(0,_i);
                }
            }
            else
            {
                if (i is DialogueChoice)
                {
                    DialogueChoice _i = (DialogueChoice)i;

                    bool hasBeenSet = false;
                    foreach (DialogueChoiceBlock ii in currentTitleBlock.dialogueChoiceBlocks)
                    {
                        // Check indentation
                        if (ii.choiceTabCount == _i.tabCount && !ii.dialogueChoices.Contains(_i))
                        {
                            ii.dialogueChoices.Insert(0,_i);
                            hasBeenSet = true;
                        }
                    }

                    if (!hasBeenSet)
                    {
                        currentChoiceBlock = new DialogueChoiceBlock();
                        currentTitleBlock.dialogueChoiceBlocks.Add(currentChoiceBlock);

                        currentChoiceBlock.choiceTabCount = _i.tabCount;

                        currentChoiceBlock.dialogueChoices.Insert(0,_i);
                    }
                }
            }

            if (currentConditionBlock == null)
            {

            }
            else
            {

            }

        }
    }
}
