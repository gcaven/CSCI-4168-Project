using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;
    private Vector2 previousVelocity;

	void FixedUpdate () {
        Rigidbody2D playerBody = GetComponent<Rigidbody2D>();
        Vector2 velocity = InputManager.PrimaryAxis() * movementSpeed * Time.deltaTime;
        if (velocity.x != 0 && velocity.y != 0) {
            velocity.x = velocity.x * 0.7071f;
            velocity.y = velocity.y * 0.7071f;
        }
        if (InputManager.FaceButtonBottom()) {
            Debug.Log(InputManager.PrimaryAxis());
        }
        playerBody.velocity = velocity;
    }
}