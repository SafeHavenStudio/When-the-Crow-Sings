using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTabTop : JournalTab
{
    public Sprite tabColorSprite;
    public Image tabImage;


    private void Awake()
    {
        tabImage.sprite = tabColorSprite;
    }
}
