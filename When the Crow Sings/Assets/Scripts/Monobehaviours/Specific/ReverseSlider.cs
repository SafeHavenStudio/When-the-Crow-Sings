using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReverseSlider : MonoBehaviour
{
    public Slider reversedSlider;

    public float minValue = .01f;
    public float maxValue = .1f;
    public float invertedValue;
    public GameSettings gameSettings;

    void Start()
    {
        reversedSlider.minValue = minValue;
        reversedSlider.maxValue = maxValue;

        reversedSlider.onValueChanged.AddListener(OnSliderChanged);
    }

    public float GetVisualValue(float inverted)
    {
        return reversedSlider.maxValue - inverted + reversedSlider.minValue;
    }

    void OnSliderChanged(float value)
    {
        //Invert the value: max + min - current slider value
        invertedValue = maxValue + minValue - value;

        Debug.Log("Inverted Value: " + invertedValue);

        gameSettings.textSpeed = invertedValue;
    }

}
