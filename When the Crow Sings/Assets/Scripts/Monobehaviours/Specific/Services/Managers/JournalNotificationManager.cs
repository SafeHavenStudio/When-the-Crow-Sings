using ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalNotificationManager : MonoBehaviour
{
    public GameSignal popupUpdateMessageSignal;
    public GameObject notificationPrefab;
    public float verticalOffset = 20f;

    public Canvas canvas;

    private void Start()
    {
        SaveDataAccess.popupUpdateMessageSignal = popupUpdateMessageSignal;
    }

    public void ShowNewNotification(SignalArguments args)
    {
        GameObject notification = Instantiate(notificationPrefab);
        notification.transform.SetParent(canvas.transform, false);
        notification.transform.localPosition = new Vector3(notification.transform.localPosition.x, verticalOffset, notification.transform.localPosition.z);
    }

}
