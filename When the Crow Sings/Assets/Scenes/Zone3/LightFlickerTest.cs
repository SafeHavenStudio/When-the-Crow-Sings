using UnityEngine;

public class LightFlickerTest : MonoBehaviour
{
    public Light myLight;
    public float maxInterval = 2;
    public float maxBurst = 1f;
    public float maxFlicker = 0.2f;

    float defaultIntensity;
    bool isFlickering;
    float timer;
    float interval;

    private void Start()
    {
        defaultIntensity = myLight.intensity;
        interval = Random.Range(0, maxInterval);
    }

    void Update()
    {
        if (!isFlickering)
        {
            timer += Time.deltaTime;
        }

        if (timer > interval)
        {
            interval = Random.Range(0, maxInterval);
            timer = 0;
            StartCoroutine(FlickerLight(Random.Range(0, maxBurst)));
        }
    }

    System.Collections.IEnumerator FlickerLight(float duration)
    {
        isFlickering = true;
        float totalTime = 0;
        float flickerTimer = 0;
        float flickerInterval = Random.Range(0, maxFlicker);

        while (totalTime < duration)
        {
            totalTime += Time.deltaTime;
            flickerTimer += Time.deltaTime;

            if (flickerTimer > flickerInterval)
            {
                myLight.intensity = Random.Range(0.5f, defaultIntensity);
                flickerInterval = Random.Range(0, maxFlicker);
                flickerTimer = 0;
            }
            yield return null;
        }

        myLight.intensity = defaultIntensity;
        isFlickering = false;
    }
}
