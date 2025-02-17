using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public partial class DialogueParser
{
    private void IdentifyAndAssignDialogueLines(string text)
    {
        raw_lines = text.Split("\n");

        for (int i = 0; i < raw_lines.Length; i++)
        {
            int myTabCount = -1;
            trimmedLine = raw_lines[i];

            CountTabs(i, ref myTabCount);

            // Skip empty lines.
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("//"))
            {
                dialogueResource.dialogueLines.Add(new DialogueEmpty());
                continue;
            }

            CheckWhichLineType(i, myTabCount);
        }
    }
    private void CountTabs(int i, ref int myTabCount) // The ref int is intentional here.
    {
        // Count the number of indents/tabs.
        bool hasFinishedCountingTabs = false;
        myTabCount = 0;
        while (!hasFinishedCountingTabs)
        {
            if (trimmedLine.StartsWith('\t'))
            {
                myTabCount = myTabCount + 1;
                trimmedLine = trimmedLine.Remove(0, 1); // Remove the tab before checking for any more.
            }
            else if (trimmedLine.StartsWith("    "))
            {
                myTabCount = myTabCount + 1;
                trimmedLine = trimmedLine.Remove(0, 4); // Remove the 4 spaces before checking for any more.
                Debug.LogError("This dialogue contains spaces instead of tabs!");
            }
            else
            {
                hasFinishedCountingTabs = true;
            }
        }

        // Actually trim the line (couldn't do it earlier because of the tab stuff)
        trimmedLine = trimmedLine.Trim();
    }

    private void CheckWhichLineType(int i, int myTabCount) // Parse the dialogue line type.
    {
        // if
        if (CheckLine_IsTitle(i, myTabCount)) return;

        // else if, else if, else if, else if, else if...
        if (CheckLine_IsGoto(myTabCount)) return;
        if (CheckLine_IsChoice(i, myTabCount)) return;
        if (CheckLine_IsConditional(i, myTabCount)) return;
        if (CheckLine_IsMutation(myTabCount)) return;

        // else
        CheckLine_IsNormalLine(myTabCount);
    }

    private bool CheckLine_IsTitle(int i, int myTabCount) // Title
    {
        if (trimmedLine.StartsWith('~'))
        {
            DialogueTitle newLine = new DialogueTitle();
            newLine.tabCount = myTabCount;

            trimmedLine = Regex.Replace(trimmedLine, @"\s+", "");
            trimmedLine = Utilities.RemoveFirstOccurence("~", trimmedLine);

            newLine.titleName = trimmedLine;
            newLine.titleIndex = i;

            dialogueResource.dialogueLines.Add(newLine);

            if (dialogueResource.dialogueLines.OfType<DialogueTitle>().ToList().Count(x => x.titleName == newLine.titleName) > 1)
            {
                throw new System.Exception("CHELLE SERIOUSLY I WARNED YOU ABOUT THIS! Error: Multiple titles with the same name detected in the same document. >:(");
            }
            
            return true;
        }
            return false;
    }

    private bool CheckLine_IsGoto(int myTabCount) // GoTo
    {
        if (trimmedLine.StartsWith("=>"))
        {
            DialogueGoto newLine = new DialogueGoto();
            newLine.tabCount = myTabCount;

            trimmedLine = Regex.Replace(trimmedLine, @"\s+", ""); // Remove white space
            trimmedLine = Utilities.RemoveFirstOccurence("=>", trimmedLine);

            if (trimmedLine == "END")
            {
                newLine.isEnd = true;
            }
            else
            {
                newLine.isEnd = false;
                newLine.gotoTitleName = trimmedLine;
            }

            dialogueResource.dialogueLines.Add(newLine);
            return true;
        }
        return false;
    }

    private bool CheckLine_IsChoice(int i, int myTabCount) // Choice
    {
        if (trimmedLine.StartsWith('-'))
        {
            DialogueChoice newLine = new DialogueChoice();

            trimmedLine = Utilities.RemoveFirstOccurence("- ", trimmedLine);

            newLine.tabCount = myTabCount;
            newLine.choiceText = trimmedLine;
            newLine.choiceIndex = i;

            dialogueResource.dialogueLines.Add(newLine);
            
            return true;
        }
        return false;
    }
  

    private bool CheckLine_IsConditional(int i, int myTabCount) // Conditional
    {
        if (
            (trimmedLine.StartsWith("if ") || trimmedLine.StartsWith("elif ") || trimmedLine.StartsWith("else"))
                    && trimmedLine.EndsWith(":"))
        {
            DialogueCondition newLine = new DialogueCondition();
            newLine.tabCount = myTabCount;
            newLine.conditionIndex = i;
            //Debug.Log("We've done a conditional line!" + i);

            PrepareConditional(trimmedLine, ref newLine);

            dialogueResource.dialogueLines.Add(newLine);

            return true;
        }
        return false;
    }

    private bool CheckLine_IsMutation(int myTabCount) // Mutation
    {
        if ((trimmedLine.StartsWith("call ") || trimmedLine.StartsWith("set ") || trimmedLine.StartsWith("emit ") )
            && trimmedLine.EndsWith(';'))
        {
            DialogueMutation newLine = new DialogueMutation();
            newLine.tabCount = myTabCount;

            trimmedLine = Regex.Replace(trimmedLine, @"\s+", ""); // Remove white space
            trimmedLine = trimmedLine.TrimEnd(';');
            if (trimmedLine.StartsWith("set"))
            {
                newLine.actionType = DialogueMutation.ActionType.SET;
                trimmedLine = Utilities.RemoveFirstOccurence("set", trimmedLine);

                // Check operator.
                if (trimmedLine.Contains("+="))
                {
                    newLine.operatorType = DialogueMutation.OperatorType.PLUS_EQUALS;
                    string[] splits = trimmedLine.Split(new string[] { "+=" }, System.StringSplitOptions.None);

                    newLine.actionKey = splits[0];
                    newLine.SetValue(splits[1]);
                }
                else if (trimmedLine.Contains("-="))
                {
                    newLine.operatorType = DialogueMutation.OperatorType.MINUS_EQUALS;
                    string[] splits = trimmedLine.Split(new string[] { "-=" }, System.StringSplitOptions.None);

                    newLine.actionKey = splits[0];
                    newLine.SetValue(splits[1]);
                }
                else if (trimmedLine.Contains("="))
                {
                    newLine.operatorType = DialogueMutation.OperatorType.EQUALS;
                    string[] splits = trimmedLine.Split('=');

                    newLine.actionKey = splits[0];
                    newLine.SetValue(splits[1]);
                }

            }
            else if (trimmedLine.StartsWith("emit"))
            {
                newLine.actionType = DialogueMutation.ActionType.EMIT;
                trimmedLine = Utilities.RemoveFirstOccurence("emit", trimmedLine);

                newLine.SetValue(trimmedLine); // Theorhetically all that remains should be a signal name (or possibly index).
            }
            else if (trimmedLine.StartsWith("call"))
            {
                newLine.actionType = DialogueMutation.ActionType.CALL;
                trimmedLine = Utilities.RemoveFirstOccurence("call", trimmedLine);

                newLine.SetValue(trimmedLine); // Theorehtically all that remains should be a method name.
            }
            else
            {
                newLine.actionType = DialogueMutation.ActionType.CHELLE_YOU_STINKY;
            }
            
            

            
            //newLine.actionValue = splits[1];

            dialogueResource.dialogueLines.Add(newLine);

            return true;
        }
        return false;
    }

    private void CheckLine_IsNormalLine(int myTabCount)
    {
        DialogueResponse newLine = new DialogueResponse();
        newLine.tabCount = myTabCount;
        if (trimmedLine.Split(":").Length > 1)
        {
            string[] split = trimmedLine.Split(":", 2);
            newLine.dialogue = string.Join("", split.Skip(1));
            newLine.dialogue = newLine.dialogue.Trim();

            if (split[0].Split('_').Length > 1)
            {
                string[] split2 = split[0].Split('_');
                newLine.characterName = split2[0];
                if (split2[1] == "Happy") newLine.characterEmotion = Constants.EMOTIONS.HAPPY;
                else if (split2[1] == "Angry") newLine.characterEmotion = Constants.EMOTIONS.ANGRY;
                else if (split2[1] == "Sad") newLine.characterEmotion = Constants.EMOTIONS.SAD;
                else if (split2[1] == "Scared") newLine.characterEmotion = Constants.EMOTIONS.SCARED;
                else if (split2[1] == "Bs") newLine.characterEmotion = Constants.EMOTIONS.BS;
                else if (split2[1] == "Smile") newLine.characterEmotion = Constants.EMOTIONS.SMILE;

                else if (split2[1] == "Stare") newLine.characterEmotion = Constants.EMOTIONS.STARE;
                else if (split2[1] == "Worried") newLine.characterEmotion = Constants.EMOTIONS.WORRIED;
                else if (split2[1] == "Thinking") newLine.characterEmotion = Constants.EMOTIONS.THINKING;

                else if (split2[1] == "Teasing") newLine.characterEmotion = Constants.EMOTIONS.TEASING;
                else if (split2[1] == "Awkward") newLine.characterEmotion = Constants.EMOTIONS.AWKWARD;

                else if (split2[1] == "Wink") newLine.characterEmotion = Constants.EMOTIONS.WINK;
                else if (split2[1] == "Disgruntled") newLine.characterEmotion = Constants.EMOTIONS.DISGRUNTLED;

                else if (split2[1] == "Shiver") newLine.characterEmotion = Constants.EMOTIONS.SHIVER;
                else if (split2[1] == "Excited") newLine.characterEmotion = Constants.EMOTIONS.EXCITED;

                else if (split2[1] == "Dazed") newLine.characterEmotion = Constants.EMOTIONS.DAZED;
                else if (split2[1] == "Panicked") newLine.characterEmotion = Constants.EMOTIONS.PANICKED;
                else if (split2[1] == "MildSurprise") newLine.characterEmotion = Constants.EMOTIONS.MILD_SURPRISE;
                else if (split2[1] == "Sigh") newLine.characterEmotion = Constants.EMOTIONS.SIGH;
                else if (split2[1] == "Annoyed") newLine.characterEmotion = Constants.EMOTIONS.ANNOYED;

                else if (split2[1] == "Intelligent") newLine.characterEmotion = Constants.EMOTIONS.INTELLIGENT;
                else if (split2[1] == "Startled") newLine.characterEmotion = Constants.EMOTIONS.STARTLED;

                else if (split2[1] == "Stunned") newLine.characterEmotion = Constants.EMOTIONS.STUNNED;
                else if (split2[1] == "Exhausted") newLine.characterEmotion = Constants.EMOTIONS.EXHAUSTED;

                else if (split2[1] == "NOPORTRAIT") newLine.characterEmotion = Constants.EMOTIONS.NO_PORTRAIT;

                else newLine.characterEmotion = Constants.EMOTIONS.DEFAULT;

            }
            else
            {
                newLine.characterName = split[0];
                newLine.characterEmotion = Constants.EMOTIONS.DEFAULT;
            }
        }
        else
        {
            newLine.characterName = "";
            newLine.dialogue = trimmedLine;
        }
        dialogueResource.dialogueLines.Add(newLine);
    }


  

}
