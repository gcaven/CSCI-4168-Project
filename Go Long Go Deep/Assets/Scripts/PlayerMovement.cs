using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;
    public float speedBurstMultiplier;

    private float timeLeftOnSpeedBurst;
    private float actionCooldown;
    private Vector2 previousMovement;

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
                if (InputManager.FaceButtonBottom() || Input.GetKeyDown(KeyCode.Q)) {
                    if (PlayerStatus.getHasFootball())
                    {
                        PlayerStatus.setHasFootball(false);
                        // Throw the ball
                        GameObject newBall = (GameObject)Instantiate(Resources.Load("Football/Football"));
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
        playerBody.velocity = velocity;
    }
}