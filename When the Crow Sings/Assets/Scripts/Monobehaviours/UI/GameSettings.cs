using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    //Keep between 1 and 3
    public Slider qteSpeedSlider;
    public TimingMeterQTE[] qtes;

    private void Start()
    {
        int savedSpeed = PlayerPrefs.GetInt("qteSpeed", 1);
        qteSpeedSlider.value = savedSpeed;

        foreach (var qte in qtes)
            qte.speed = savedSpeed;

        //Handle slider changes
        qteSpeedSlider.onValueChanged.AddListener(delegate { TimingMeterSpeed(); });
    }

    public void TimingMeterSpeed()
    {
        int newSpeed = (int)qteSpeedSlider.value;

        foreach(var qte in qtes)
            qte.speed = (int)qteSpeedSlider.value;

        PlayerPrefs.SetInt("qteSpeed", newSpeed);
        PlayerPrefs.Save();

        Debug.Log("QTE Speed set to " + newSpeed);
    }
}
