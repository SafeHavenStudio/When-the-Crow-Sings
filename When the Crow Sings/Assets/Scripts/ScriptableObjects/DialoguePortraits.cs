using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu]
public class DialoguePortraits : ScriptableObject
{
    public DialogueManager manager;

    public List<Sprite> chancePortraits;
    public List<Sprite> theodorePortraits;
    public List<Sprite> philPortraits;
    public List<Sprite> faridaPortraits;
    public List<Sprite> angelPortraits;
    public List<Sprite> calebPortraits;
    public List<Sprite> beauPortraits;
    public List<Sprite> quinnPortraits;
    public List<Sprite> jazmynePortraits;
    public List<Sprite> fransiscoPortraits;
    public List<Sprite> yulePortraits;
    public List<Sprite> itemPortraits;

    public bool isChance;
    public bool isTheodore;
    public bool isPhil;
    public bool isFarida;
    public bool isAngel;
    public bool isCaleb;
    public bool isBeau;
    public bool isQuinn;
    public bool isJazmyne;
    public bool isFrancisco;
    public bool isYule;

    public int combinedConditions;

    void start()
    {
        manager = FindObjectOfType<DialogueManager>();
        if (manager == null) Debug.Log("Manager is null");

        combinedConditions = (isChance ? 1 : 0) | (isTheodore ? 2 : 0) | (isPhil ? 4 : 0) | (isFarida ? 8 : 0) | (isAngel ? 16 : 0)
            | (isCaleb ? 32 : 0) | (isBeau ? 64 : 0) | (isQuinn ? 128 : 0) | (isJazmyne ? 256 : 0) | (isFrancisco ? 512 : 0) | (isYule ? 1024 : 0);
    }


    public Sprite GetPortrait(string characterName, Constants.EMOTIONS emotion)
    {
        List<Sprite> portraits = null;

        //Reset all conditions
        isChance = isTheodore = isPhil = isFarida = isAngel = isCaleb = isBeau = isQuinn = isJazmyne = isFrancisco = isYule = false;

        switch (characterName)
        {
            case "Chance":
                portraits = chancePortraits;
                isChance = true;
                break;
            case "Theodore":
                portraits = theodorePortraits;
                isTheodore = true;
                break;
            case "Phil":
                portraits = philPortraits;
                isPhil = true;
                break;
            case "Farida":
                portraits = faridaPortraits;
                isFarida = true;
                break;
            case "Angel":
                portraits = angelPortraits;
                isAngel = true;
                break;
            case "Caleb":
                portraits = calebPortraits;
                isCaleb = true;
                break;
            case "Beau":
                portraits = beauPortraits;
                isBeau = true;
                break;
            case "Quinn":
                portraits = quinnPortraits;
                isQuinn = true;
                break;
            case "Jazmyne":
                portraits = jazmynePortraits;
                isJazmyne = true;
                break;
            case "Francisco":
                portraits = fransiscoPortraits;
                isFrancisco = true;
                break;
            case "Yule":
                portraits = yulePortraits;
                isYule = true;
                break;
            case "Item":
                portraits = itemPortraits;
                break;
            default:
                portraits = new List<Sprite>();
                break;
        }

        //Recalculate combined conditions immediately
        combinedConditions = (isChance ? 1 : 0) | (isTheodore ? 2 : 0) | (isPhil ? 4 : 0) | (isFarida ? 8 : 0) | (isAngel ? 16 : 0)
            | (isCaleb ? 32 : 0) | (isBeau ? 64 : 0) | (isQuinn ? 128 : 0) | (isJazmyne ? 256 : 0) | (isFrancisco ? 512 : 0) | (isYule ? 1024 : 0);
        //Debug.Log($"Updated combinedConditions: {combinedConditions}");


        if (portraits.Count != 0)
        {
            return portraits[(int)emotion];
        }
        else
        {
            return null;
        }
        
    }
}
