using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMarkerHolder : MonoBehaviour
{
    public GameStateManager gameStateManager;
    Vector2 playerOffset;

    public GameObject playerMarker;

    [SerializeField]
    float offsetScaling = 3.8f;

    // Update is called once per frame
    void Update()
    {
        if (gameStateManager.currentLevelDataLVL.isExterior) playerOffset = new Vector2(
            gameStateManager.playerContent.transform.position.x - gameStateManager.currentLevelDataLVL.transform.position.x,
            gameStateManager.playerContent.transform.position.z - gameStateManager.currentLevelDataLVL.transform.position.z);
        else playerOffset = new Vector2(gameStateManager.currentLevelDataLVL.transform.position.x, gameStateManager.currentLevelDataLVL.transform.position.z);
        

        playerMarker.GetComponent<RectTransform>().anchoredPosition = new Vector3(playerOffset.x,playerOffset.y,0) * offsetScaling * gameStateManager.currentLevelDataLVL.relativeMapScale;
    }
}
