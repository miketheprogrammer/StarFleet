using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCamera2D : MonoBehaviour
{
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
    [SerializeField]
    float maxZoom = 5;
    [SerializeField]
    float minZoom = 1;

    [SerializeField]
    GameObject[] targets;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    Vector2 screenDeadZone = new Vector2(.2f, .2f);

    [SerializeField]
    CameraFocusStrategy focusStrategy = CameraFocusStrategy.SingleTargetFocusCenterReset;

    [SerializeField]
    float smoothness = .3f;

    private bool mustFocus = false;
    private Vector3 centerScreen = new Vector3(.5f,.5f,0);
    // Start is called before the first frame update
    private Vector3 player2DPosition = new Vector3(0f, 0f, 0f);
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll > 0.01f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - .5f, .1f, 20f);
        }
        if (scroll < -0.01f)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + .5f, .1f, 20f);
        }

        if (focusStrategy == CameraFocusStrategy.SingleTargetFocusFollow)
        {
            if (player != null)
            {
                Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
            }
            return;
        }

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
