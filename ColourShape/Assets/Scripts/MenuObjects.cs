using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjects : MonoBehaviour {

    const float MAX_PULSE = 18f;
    const float MIN_PULSE = 13f;
    const float PULSE_SPEED = 4f;
    const float MAX_Y = 1f;
    const float BOUNCE_SPEED = 2f;

    Vector3 positionStorage;
    float yPos;

    // Use this for initialization
    void Start () {
        yPos = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<Light>().range = MIN_PULSE + Mathf.PingPong(Time.time * PULSE_SPEED, MAX_PULSE - MIN_PULSE);
        positionStorage = transform.position;
        positionStorage.y = yPos + Mathf.PingPong(Time.time * BOUNCE_SPEED, MAX_Y);
        transform.position = positionStorage;
    }
}
