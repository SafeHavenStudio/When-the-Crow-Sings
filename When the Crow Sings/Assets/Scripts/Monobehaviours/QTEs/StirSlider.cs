using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StirSlider : MonoBehaviour
{
    public StirringQTE stirringQTE;
    public Image fillImage;
    public Slider slider;
    float fillValue = 0f;
    private float meterSpeed = 15f;
    private float decaySpeed = 1f;

    private void Awake()
    {
        stirringQTE = FindObjectOfType<StirringQTE>();
        fillImage = GetComponentInChildren<Image>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Increase smoothly when scoring
        fillValue = Mathf.MoveTowards(fillValue, stirringQTE.score, Time.deltaTime * meterSpeed);

        //Gradually decrease if no input
        if (stirringQTE.score == fillValue)
            fillValue = Mathf.MoveTowards(fillValue, 0, Time.deltaTime * decaySpeed);

        slider.value = fillValue;

        /*if (stirringQTE.complete)
        {
            fillImage.color = Color.green;
        }*/

        if (stirringQTE.failed)
        {
            fillImage.color = Color.red;
        }
    }
}
