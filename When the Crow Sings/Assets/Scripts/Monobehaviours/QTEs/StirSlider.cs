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

    private void Awake()
    {
        stirringQTE = FindObjectOfType<StirringQTE>();
        fillImage = GetComponentInChildren<Image>();
        slider = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //Sets slider to fill adjacent to stirringqte.score
        fillValue = Mathf.MoveTowards(fillValue, stirringQTE.score, Time.deltaTime * meterSpeed);
        slider.value = fillValue;

        if (stirringQTE.complete == true)
        {
            fillImage.color = Color.green;
            fillValue = slider.value;
            Debug.Log("QTE COLOR CHANGE:");
        }

        if (stirringQTE.failed == true)
        {
            fillImage.color = Color.red;
            fillValue = slider.value;
            Debug.Log("QTE COLOR CHANGE:");
        }
    }
}
