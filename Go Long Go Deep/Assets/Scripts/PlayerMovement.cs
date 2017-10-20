using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;

	void Update () {
        Rigidbody2D playerBody = GetComponent<Rigidbody2D>();
        Vector2 velocity = Vector2.zero;
		if (Input.GetKey(KeyCode.D)) {
            velocity.x = movementSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.A)) {
            velocity.x = 0 - (movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W)) {
            velocity.y = movementSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S)) {
            velocity.y = 0 - (movementSpeed * Time.deltaTime);
        }
        if (velocity.x != 0 && velocity.y != 0) {
            velocity.x = velocity.x * 0.7071f;
            velocity.y = velocity.y * 0.7071f;
        }
        playerBody.velocity = velocity;
    }
}