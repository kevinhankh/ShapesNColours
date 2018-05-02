using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
    const float MAX_PULSE = 18f;
    const float MIN_PULSE = 13f;
    const float PULSE_SPEED = 4f;
    const float MAX_Y = 1f;
    const float BOUNCE_SPEED = 2f;
    int score = 0;
    float yPos;
    Vector3 positionStorage;
    GameObject GameManager;

    // Use this for initialization
    void Start () {
        GameManager = GameObject.FindWithTag("GameManager");
        yPos = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetComponent<Light>().range = MIN_PULSE + Mathf.PingPong(Time.time * PULSE_SPEED, MAX_PULSE - MIN_PULSE);
        positionStorage = transform.position;
        positionStorage.y = yPos + Mathf.PingPong(Time.time * BOUNCE_SPEED, MAX_Y);
        transform.position = positionStorage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            switch (tag)
            {
                case "GoalCube":
                    if (collision.gameObject.tag == "ProjectileCube")
                        score++;
                    break;
                case "GoalSphere":
                    if (collision.gameObject.tag == "ProjectileSphere")
                        score++;
                    break;
                case "GoalTriangle":
                    if (collision.gameObject.tag == "ProjectileTriangle")
                        score++;
                    break;
            }

            if (collision.gameObject.GetComponent<Light>().color == GetComponent<Light>().color)
                score++;
            GameManager.GetComponent<GameManager>().UpdateGoal(score);
            Destroy(gameObject);
        }
    }
}
