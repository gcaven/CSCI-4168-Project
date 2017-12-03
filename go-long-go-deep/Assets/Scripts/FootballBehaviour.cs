using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballBehaviour : MonoBehaviour {
	// Private vars to store ball state
    private bool isMoving;
    private Vector2 direction;
    // Public vars for ball behaviour
    public int movementSpeed;
    public float movementDistance;
    public int damage;

	/*
	 * We use this to initialize the ball with some parameters to define initial position and direction
	 * The usual Start() does not allow for parameters
	 */
    public void Initialize(Vector2 position, Vector2 velocity) {
        isMoving = true;
		GetComponent<Rigidbody2D>().position = NormalizePositionForDirection(position, velocity);
        setDirection(velocity);
    }

	/*
	 * Use the starting velocity to determine ball direction
	 * Thanks Pythagorus, you were a pretty cool dude
	 */
	void setDirection(Vector2 source) {
		// If the player isn't moving
		if (source == Vector2.zero) {
			// Pick an arbitrary direction
			direction = Vector2.right;
		}
		else {
			direction = Vector2.zero;
			// Let's do some Pythagorus :)
			float hyp = Mathf.Sqrt((source.x * source.x) + (source.y * source.y));
			direction.x = source.x / hyp;
			direction.y = source.y / hyp;
		}
	}

	/*
	 * Player position if defined at the top left of their bounding box
	 * We have to adjust the ball spawn postion based on movement to avoid getting the player
	 * caught on the ball, and make the spawn point seem natural and correct.
	 * The values chosen are arbitrary based on what look correct in motion
	 */
	Vector2 NormalizePositionForDirection(Vector2 position, Vector2 velocity) {
		Vector2 finalPosition = new Vector2(position.x, position.y + 0.1f);
		if (velocity.x > 0) {
			finalPosition.x += 0.2f;
		} else if (velocity.x < 0) {
			finalPosition.x -= 0.2f;
		}
		if (velocity.y > 0) {
			finalPosition.y += 0.2f;
		} else if (velocity.y < 0) {
			finalPosition.y -= 1.2f;
		}
		return finalPosition;
	}
	
	/*
	 * Once per frame, if the ball is moving, continue its motion
	 * If the defined distance has been covered, stop moving and set the isMoving
	 * flag to false to prevent any future movement
	 */
	void FixedUpdate () {
        Rigidbody2D footballBody = GetComponent<Rigidbody2D>();

        if (isMoving) {
            Vector2 movementVector = direction * movementSpeed;
            footballBody.velocity = movementVector;
            movementDistance -= Time.deltaTime;
            if(movementDistance <= 0f) {
                isMoving = false;
            }
        }
        else {
            footballBody.velocity = Vector2.zero;
        }
    }

	/*
	 * Football prefab is wrapped in a trigger
	 * If the player enters the trigger when the ball is not moving, they pick it up
	 * If an enemy enters the trigger when the ball is moving, stop moving
	 * The enemy has its own trigger and contains the logic for registering a hit
	 * from the ball and taking damage.
	 */
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.name == "Player" && !isMoving) {
            Player.setHasFootball(true);
            Destroy(this.gameObject);
        }
		else if(other.gameObject.tag == "Wall" || other.gameObject.tag == "Enemy") {
            isMoving = false;
        }
    }
}
