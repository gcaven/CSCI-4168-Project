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
	void Start () {
        isMoving = true;
        GameObject player = PlayerStatus.getPlayer();
        Vector2 pos = player.transform.position;
        GetComponent<Rigidbody2D>().position = pos;
        setDirection(player.GetComponent<Rigidbody2D>().velocity);
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
            PlayerStatus.setHasFootball(true);
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
