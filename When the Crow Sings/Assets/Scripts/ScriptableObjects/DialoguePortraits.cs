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

    public enum WhoIsTalking
    {
        Chance,
        Theodore,
        Phil,
        Farida,
        Angel,
        Caleb,
        Beau,
        Quinn,
        Jazmyne,
        Francisco,
        Yule,
        None
    }

    public WhoIsTalking whoIsTalking = WhoIsTalking.Chance;


    void start()
    {
        manager = FindObjectOfType<DialogueManager>();
        if (manager == null) Debug.Log("Manager is null");
    }


    public Sprite GetPortrait(string characterName, Constants.EMOTIONS emotion)
    {
        List<Sprite> portraits = null;

        switch (characterName)
        {
            case "Chance":
                portraits = chancePortraits;
                whoIsTalking = WhoIsTalking.Chance;
                break;
            case "Theodore":
                portraits = theodorePortraits;
                whoIsTalking = WhoIsTalking.Theodore;
                break;
            case "Phil":
                portraits = philPortraits;
                whoIsTalking = WhoIsTalking.Phil;
                break;
            case "Farida":
                portraits = faridaPortraits;
                whoIsTalking = WhoIsTalking.Farida;
                break;
            case "Angel":
                portraits = angelPortraits;
                whoIsTalking = WhoIsTalking.Angel;
                break;
            case "Caleb":
                portraits = calebPortraits;
                whoIsTalking = WhoIsTalking.Caleb;
                break;
            case "Beau":
                portraits = beauPortraits;
                whoIsTalking = WhoIsTalking.Beau;
                break;
            case "Quinn":
                portraits = quinnPortraits;
                whoIsTalking = WhoIsTalking.Quinn;
                break;
            case "Jazmyne":
                portraits = jazmynePortraits;
                whoIsTalking = WhoIsTalking.Jazmyne;
                break;
            case "Francisco":
                portraits = fransiscoPortraits;
                whoIsTalking = WhoIsTalking.Francisco;
                break;
            case "Yule":
                portraits = yulePortraits;
                whoIsTalking = WhoIsTalking.Yule;
                break;
            case "Item":
                portraits = itemPortraits;
                whoIsTalking = WhoIsTalking.None;
                break;
            default:
                portraits = new List<Sprite>();
                whoIsTalking = WhoIsTalking.None;
                break;
        }


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
