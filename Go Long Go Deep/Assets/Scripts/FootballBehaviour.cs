using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballBehaviour : MonoBehaviour {

    private bool isMoving;
    private Vector2 direction;
    
    public int movementSpeed;
    public float movementDistance;
    public int damage;

	// Use this for initialization
    public void Initialize(Vector2 position, Vector2 velocity) {
        isMoving = true;
		GetComponent<Rigidbody2D>().position = NormalizePositionForDirection(position, velocity);
        setDirection(velocity);
    }

	// Player position if defined at the top left of their bounding box
	// We have to adjust the ball spawn postion based on movement to avoid getting the player
	// caught on the ball, and make the spawn point seem natural and correct.
	// The values chosen are arbitrary based on what look correct in motion
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
	
	// Update is called once per frame
	void FixedUpdate () {
        Rigidbody2D footballBody = GetComponent<Rigidbody2D>();

        if (isMoving)
        {
            Vector2 movementVector = direction * movementSpeed;
            footballBody.velocity = movementVector;
            movementDistance -= Time.deltaTime;
            if(movementDistance <= 0f)
            {
                isMoving = false;
            }
        }
        else
        {
            footballBody.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "Player" && !isMoving)
        {
            PlayerMovement.setHasFootball(true);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.tag == "Wall")
        {
            print("YOOOOO");
            isMoving = false;
        }
        else
        {
            print(SortingLayer.IDToName(other.gameObject.layer));
        }
    }

    void setDirection(Vector2 source)
    {
        // If the player isn't moving
        if (source == Vector2.zero)
        {
            // Pick an arbitrary direction
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.zero;
            // Let's do some Pythagorus :)
            float hyp = Mathf.Sqrt((source.x * source.x) + (source.y * source.y));
            direction.x = source.x / hyp;
            direction.y = source.y / hyp;
        }
    }
}
