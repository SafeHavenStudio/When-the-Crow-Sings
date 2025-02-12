using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DangerOverlay : MonoBehaviour//, IService
{
    public float dangerAlpha = 0.0f;
    //public void RegisterSelfAsService()
    //{
    //    ServiceLocator.Register(this);
    //}

    Color actualColor = Color.white;
    void Awake()
    {
        string hexColor = "#652858";
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexColor, out Color _color)) actualColor = _color;
    }

    public float lerpSpeed = 1f;
    public float colorLerpSpeed = 20f;

    public Image overlayImage;

    EnemyController enemyController;


    // Update is called once per frame
    void Update()
    {
        if (ServiceLocator.CheckIfServiceExists<EnemyController>())
        {
            enemyController = ServiceLocator.Get<EnemyController>();

            if (enemyController.canSeePlayer || enemyController.IsChasingPlayer) dangerAlpha = Mathf.Lerp(dangerAlpha, .5f, Time.deltaTime * lerpSpeed);
            else dangerAlpha = Mathf.Lerp(dangerAlpha, 0f, Time.deltaTime * lerpSpeed);
            GetComponent<CanvasGroup>().alpha = dangerAlpha;

            if (enemyController.IsChasingPlayer)
            {
                overlayImage.color = Color.Lerp(overlayImage.color, Color.red, Time.deltaTime * colorLerpSpeed);
            }
            else
            {
                overlayImage.color = Color.Lerp(overlayImage.color, actualColor, Time.deltaTime * colorLerpSpeed);
            }
            return;
        }
        else
        {
            dangerAlpha = Mathf.Lerp(dangerAlpha, 0f, Time.deltaTime * lerpSpeed);
            GetComponent<CanvasGroup>().alpha = dangerAlpha;
            overlayImage.color = Color.Lerp(overlayImage.color, actualColor, Time.deltaTime * colorLerpSpeed);
        }

        
    }
}
