using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	static Player instance;
	static GameObject currentRoom;
    public float movementSpeed;
    public float speedBurstMultiplier;

    private static Animator animator;
    private static bool hasFootball;
    private float timeLeftOnSpeedBurst;
    private float actionCooldown;
    private Vector2 previousMovement;

    void Start() {
        hasFootball = true;
        animator = GetComponent<Animator>(); 
		instance = this;
    }

    public static void setHasFootball(bool val) {
        hasFootball = val;
        animator.SetBool("hasBall", val);
    }

	public static Player GetPlayer() {
		return instance;	
	}

	public static GameObject GetCurrentRoom() {
		return currentRoom;
	}

	public static void setCurrentRoom(GameObject room) {
		currentRoom = room;
	}
			
    private void setAnimation(Vector2 velocity) {
        if (velocity.x != 0f || velocity.y != 0f) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }
		if (velocity.x > 0 && transform.localScale.x < 0) {
			flipX();
		} else if (velocity.x < 0 && transform.localScale.x > 0) {
			flipX();
		}
    }

	private void flipX() {
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
		
    void FixedUpdate () {
        Rigidbody2D playerBody = GetComponent<Rigidbody2D>();
        Vector2 velocity;
        if (actionCooldown > 0) {
            actionCooldown -= Time.deltaTime;
        }
        if (timeLeftOnSpeedBurst > 0) {
            timeLeftOnSpeedBurst -= Time.deltaTime;
            velocity = previousMovement * speedBurstMultiplier;
        } else {
            velocity = InputManager.PrimaryAxis() * movementSpeed * Time.deltaTime;
            if (velocity.x != 0 && velocity.y != 0) {
                velocity.x = velocity.x * 0.7071f;
                velocity.y = velocity.y * 0.7071f;
            }
            previousMovement = velocity;
            if (actionCooldown <= 0) {
                // Player is tackling / attempting to throw ball
                if (InputManager.FaceButtonBottom()) {
                    if (hasFootball)
                    {
                        setHasFootball(false);
                        // Throw the ball
                        GameObject newBall = (GameObject)Instantiate(Resources.Load("Football/Football"));
                        newBall.GetComponent<FootballBehaviour>().Initialize(GetComponent<Rigidbody2D>().position, velocity);
                    }
                    else
                    {
                        timeLeftOnSpeedBurst = 0.35f;
                    }
                    actionCooldown = 0.70f;
                    // Player is checking
                } else if (InputManager.FaceButtonRight()) {
                    actionCooldown = 0.25f;
                }
            }
        }
        setAnimation(velocity);
        playerBody.velocity = velocity;
    }
}