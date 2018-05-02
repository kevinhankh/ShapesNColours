using UnityEngine;
using System.Collections;

public class MouseLook : MonoBehaviour {
    Vector2 mouseDelta, mouseMovement, smoothV;
    public float sensitivity = 5f, smoothing = 2f;
    const float MIN_ANGLE_Y = -85.0f;
    const float MAX_ANGLE_Y = 85.0f;
    GameObject PlayerObject;

	// Use this for initialization
	void Start () {
        PlayerObject = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDelta.x, 1f / smoothing);
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDelta.y, 1f / smoothing);
        mouseMovement += smoothV;

        mouseMovement.y = Mathf.Clamp(mouseMovement.y, MIN_ANGLE_Y, MAX_ANGLE_Y);
        transform.localRotation = Quaternion.AngleAxis(-mouseMovement.y, Vector3.right);
        PlayerObject.transform.localRotation = Quaternion.AngleAxis(mouseMovement.x, PlayerObject.transform.up);
        
	}
}
