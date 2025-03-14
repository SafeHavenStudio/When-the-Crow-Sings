using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DialogueResource : ScriptableObject
{
    public TextAsset textAsset;
    //[HideInInspector]
    //public string path = "Assets/Dialogue/TestText.txt";
    [HideInInspector]
    public List<DialogueBase> dialogueLines = new List<DialogueBase>();
    [HideInInspector]
    public List<DialogueTitleBlock> dialogueTitleBlocks = new List<DialogueTitleBlock>();


    //[HideInInspector]
    //public List<DialogueTitle> dialogueTitles = new List<DialogueTitle>();
}
