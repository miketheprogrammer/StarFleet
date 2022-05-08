using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCamera2D : MonoBehaviour
{
    //Focus Strategy Types
    public enum CameraFocusStrategy
    {
        SingleTargetFocusCenterReset,
        SingleTargetFocusFollow,
        SingleTargetFocusDeadZoneMaintain,
        MultiTargetFocusCenterReset,
        MultiTargetFocusFollow,
        MultiTargetFocusDeadZoneMaintain,
        MultiTargetFieldMaintain,
    }


//Variables

    [Header("VFX Control")]
    //Max Zoom
    [SerializeField]
    float maxZoom = 5;
    //Min Zoom
    [SerializeField]
    float minZoom = 1;
    //GameObject targets, is 0
    [SerializeField]
    GameObject[] targets;
    //Player game object select field
    [SerializeField]
    public GameObject player;
    //screen Dead zone
    [SerializeField]
    Vector2 screenDeadZone = new Vector2(.2f, .2f);
    //Select Type of class based on enum types
    [SerializeField]
    CameraFocusStrategy focusStrategy = CameraFocusStrategy.SingleTargetFocusCenterReset;
    //Camera Smoothness
    [SerializeField]
    float smoothness = .3f;
    //must Focus
    private bool mustFocus = false;
    //Find Center screen based on vector 3
    private Vector3 centerScreen = new Vector3(.5f,.5f,0);
    //find player 2d pos
    private Vector3 player2DPosition = new Vector3(0f, 0f, 0f);

    //Start
    void Start()
    {
        
    }

//Update
    void Update()
    {

        //Get input from scroll wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        //if scroll is either up or down zoom in and out
        if (scroll > 0.01f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - .5f, .1f, 20f);
        }

        if (scroll < -0.01f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .5f, .1f, 20f);
        }

        //IF follow camera follow player transform 
        if (focusStrategy == CameraFocusStrategy.SingleTargetFocusFollow)
        {
            if (player != null)
            {
                Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
            }
            return;
        }

        //if camera reset reset position to position
        if (mustFocus)
        {
            if (focusStrategy == CameraFocusStrategy.SingleTargetFocusCenterReset)
            {
                player2DPosition.x = player.transform.position.x;
                player2DPosition.y = player.transform.position.y;
                player2DPosition.z = Camera.main.transform.position.z;
                Vector3 playerScreenPosition = Camera.main.WorldToViewportPoint(player.transform.position);
                playerScreenPosition.z = 0f;
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, player2DPosition, Time.deltaTime * smoothness * (playerScreenPosition.sqrMagnitude - .5f));
                if (Mathf.Approximately(Camera.main.transform.position.x, player.transform.position.x) && Mathf.Approximately(Camera.main.transform.position.y, player.transform.position.y))
                {
                    mustFocus = false;
                }
            }
        }
        
        //Check to see if player has moved and is out of bounds of parameters
        if (player != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(player.transform.position);
            
            if (screenPos.y < screenDeadZone.y || screenPos.y > (1f - screenDeadZone.y))
            {
                mustFocus = true;
            }
            if (screenPos.x < screenDeadZone.x || screenPos.x > (1f - screenDeadZone.x))
            {
                mustFocus = true;
            }
        }

    }
}
