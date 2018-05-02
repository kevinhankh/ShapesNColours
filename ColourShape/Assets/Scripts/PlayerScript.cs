using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    const float MOVE_SPEED = 0.4f;
    const float JUMP_SPEED = 1700.0f;
    const float SHOOT_SPEED = 30.0f;
    const float SHOOT_DURATION = 2.0f;
    const float SHOOT_COOLDOWN = 0.5f;
    const float VOLUME_SCALE = 1.0f;
    const int SHAPES_COLOURS = 3;
    const int INITIAL_SHAPE = 0;
    const int INITIAL_COLOUR = 2;
    const int MAX_CHOICE = 2;
    const int MIN_CHOICE = 0;

    public AudioClip changeShapeSfx;
    public AudioClip changeColourSfx;
    public AudioClip shootSfx;

    float moveForward, moveStrafe;
    float shootTimer;
    bool shootCooldown;
    int currentShape, currentColour;

    Color[] PlayerColour;
    GameObject ProjectileSpawner, ProjectileTarget;
    GameObject[] ProjectileObject;
    Vector3 shootDirection;
    AudioSource AudioPlayer;
    
    void Start ()
    {
        PlayerColour = new Color[SHAPES_COLOURS] { Color.red, Color.green, Color.blue };

        ProjectileObject = new GameObject[SHAPES_COLOURS] { GameObject.FindWithTag("ProjectileCube"), GameObject.FindWithTag("ProjectileSphere"), GameObject.FindWithTag("ProjectileTriangle") };
        ProjectileSpawner = GameObject.FindWithTag("ProjectileSpawner");
        ProjectileTarget = GameObject.FindWithTag("ProjectileTarget");

        AudioPlayer = GetComponent<AudioSource>();

        currentShape = INITIAL_SHAPE;
        currentColour = INITIAL_COLOUR;
        SetColour();

        shootTimer = SHOOT_COOLDOWN;
        shootCooldown = false;
	}
	
	void Update ()
    {
        PlayerMovement();
        ShootTimer();

        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded())
            {
                GetComponent<Rigidbody>().AddForce(Vector3.up * JUMP_SPEED);
            }
        }
        if(Input.GetButtonDown("ChangeShape"))
        {
            ChangeShape();
        }
        if(Input.GetButtonDown("ChangeColour"))
        {
            ChangeColour();
        }
        if(Input.GetButton("Fire1"))
        {
            if (!shootCooldown)
            {
                shootCooldown = true;
                Shoot();
            }
        }
    }

    void PlayerMovement()
    {
        moveForward = Input.GetAxis("Vertical") * MOVE_SPEED;
        moveStrafe = Input.GetAxis("Horizontal") * MOVE_SPEED;
        transform.Translate(moveStrafe, 0, moveForward);
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
    }

    void ChangeShape()
    {
        AudioPlayer.PlayOneShot(changeShapeSfx, VOLUME_SCALE);

        transform.GetChild(currentShape).gameObject.SetActive(false);

        if (currentShape == MAX_CHOICE)
            currentShape = MIN_CHOICE;
        else
            currentShape++;

        transform.GetChild(currentShape).gameObject.SetActive(true);

        SetColour();
    }
    
    void ChangeColour()
    {
        AudioPlayer.PlayOneShot(changeColourSfx, VOLUME_SCALE);

        if (currentColour == MAX_CHOICE)
            currentColour = MIN_CHOICE;
        else
            currentColour++;

        SetColour();
    }

    void SetColour()
    {
        transform.GetChild(currentShape).GetComponent<Light>().color = PlayerColour[currentColour];
        transform.GetChild(currentShape).GetComponent<Renderer>().material.color = PlayerColour[currentColour];
    }

    void Shoot()
    {
        AudioPlayer.PlayOneShot(shootSfx, VOLUME_SCALE);

        var projectile = (GameObject)Instantiate(ProjectileObject[currentShape], ProjectileSpawner.transform.position, ProjectileSpawner.transform.rotation);
        shootDirection = ProjectileTarget.transform.position - ProjectileSpawner.transform.position;
        projectile.GetComponent<Rigidbody>().AddForce(shootDirection * SHOOT_SPEED);
        projectile.GetComponent<Light>().color = PlayerColour[currentColour];
        Destroy(projectile, SHOOT_DURATION);
    }

    void ShootTimer()
    {
        if(shootCooldown)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0)
            {
                shootCooldown = false;
                shootTimer = SHOOT_COOLDOWN;
            }
        }
    }
}
