using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingLight : MonoBehaviour {
    private const float MAX_PULSE = 0.3f;
    private const float MIN_PULSE = 0f;
    private const float PULSE_SPEED = 0.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<Light>().intensity = MIN_PULSE + Mathf.PingPong(Time.time * PULSE_SPEED, MAX_PULSE - MIN_PULSE);
    }
}
