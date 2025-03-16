using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    //Keep between 1 and 3
    public int speed;
    public Slider qteSpeedSlider;

    private void Start()
    {
        speed = (int)qteSpeedSlider.value;
    }

    public void TimingMeterSpeed(int _speedIndex)
    {
        PlayerPrefs.SetInt("qteSpeed", _speedIndex);
        PlayerPrefs.Save();

        _speedIndex = speed; 
    }
}
