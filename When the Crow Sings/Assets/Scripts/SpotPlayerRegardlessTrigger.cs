using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotPlayerRegardlessTrigger : MonoBehaviour
{
    public EnemyController enemyController;
    private void OnTriggerEnter(Collider other)
    {
        enemyController.OnSpotPlayerRegardlessTriggerEntered(other);
    }
}
