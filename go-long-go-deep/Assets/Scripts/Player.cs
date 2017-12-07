using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Script for interpreting player input and translating it
 * onto the Player character
 */

public class Player : MonoBehaviour {
    // private state and static vars
    // can use static references since there is only one player
    private string equippedFootball;
	static Player instance;
	static GameObject currentRoom;
	private static Animator animator;
	private static bool hasFootball;
	private Rigidbody2D playerBody;
	private float timeLeftOnSpeedBurst;
	private float actionCooldown;
	private Vector2 previousMovement;
    public int gold;
	// public vars for behaviour
    public float movementSpeed;
	// public references to interface componenets modified
	// by this script.
	public Text healthText;
    public Text goldText;
	public GameObject gameOverScreen;
    public GameObject shopScreen;

	// Stats modifiable by items
//	private float speedMulitplier;
	private int totalHP;
	private int currentHP;
//	private int footballSpeedMultiplier;
//	private int footballDistanceMultiplier;
//	private int footballDamageMultiplier;
//	private bool footballAutoReturn;
//	private int checkDamageMultiplier;
	private float speedBurstMultiplier;

	/*
	 * On initialization, store variables that will need
	 * to be referenced often. GetComponent<>(); is slow, so
	 * it is better to store these refs than call it potentially
	 * every frame.
	 * Also set starting values like health etc.
	 */ 
    void Start() {
        hasFootball = true;
        equippedFootball = "Football";
        animator = GetComponent<Animator>(); 
		playerBody = GetComponent<Rigidbody2D>();
		instance = this;
//		speedMulitplier = 1;
		totalHP = 5;
		currentHP = 5;
//		footballSpeedMultiplier = 1;
//		footballDamageMultiplier = 1;
//		checkDamageMultiplier = 1;
		speedBurstMultiplier = 2f;
//		footballAutoReturn = false;
		healthText.text = currentHP + "/" + totalHP + " <3";
        goldText.text = System.Convert.ToString(gold);
        gameOverScreen.SetActive(false);
        shopScreen.SetActive(false);
    }

    public void SetEquippedFootball(string fball)
    {
        equippedFootball = fball;
    }

	/* A static function that allows other scripts to modify football
	 * possesion without having to store ref to the Player
	 */
    public static void setHasFootball(bool val) {
        hasFootball = val;
        animator.SetBool("hasBall", val);
    }

	/*
	 * Allows any script to get a ref to the player if needed
	 * (Singleton pattern)
	 */
	public static Player GetPlayer() {
		return instance;	
	}

	/*
	 * Allow any script to get the room currently occupied by the player
	 */
	public static GameObject GetCurrentRoom() {
		return currentRoom;
	}

    public GameObject GetShopScreen(){
        return shopScreen;
    }

	/*
	 * Allows any script to change the current room (Camera script)
	 */
	public static void setCurrentRoom(GameObject room) {
		currentRoom = room;
	}

	/*
	 * Allows a script with player ref to lower the player's HP
	 */
	public void takeDamage(int damage) {
		Debug.Log ("taking damage! " + Time.fixedTime);
		animator.SetTrigger ("beingHit");
		currentHP -= damage;
		setHealthUI ();
	}

	/*
	 * Updates the UI to reflect the player's current HP
	 * If the player dies, show the game over screen and pause the game
	 */
	private void setHealthUI() {
		healthText.text = currentHP + "/5 <3";
		if (currentHP <= 0) {
			gameOverScreen.SetActive(true);
			Time.timeScale = 0;
		} 
	}

	/*
	 * Main functional loop of the Player script
	 * Conditionally take actions based on input and state
	 * Use inputManager to get normalized input
	 */
    void FixedUpdate () {
        Vector2 velocity;
		// if in cooldown, decrement time remaining
        if (actionCooldown > 0) {
            actionCooldown -= Time.deltaTime;
        }
        if (timeLeftOnSpeedBurst > 0) {
            timeLeftOnSpeedBurst -= Time.deltaTime;
            velocity = previousMovement * speedBurstMultiplier;
        } else {
            velocity = InputManager.PrimaryAxis() * movementSpeed * Time.deltaTime;
			// normalize non-cardinal movement so that speed remains roughly constant
			// regardless of direction
            if (velocity.x != 0 && velocity.y != 0) {
                velocity.x = velocity.x * 0.7071f;
                velocity.y = velocity.y * 0.7071f;
            }
            previousMovement = velocity;
			// Not in a cooldown, so player is free to take actions
            if (actionCooldown <= 0) {
                // Player is tackling / attempting to throw ball
                if (InputManager.FaceButtonBottom()) {
                    if (hasFootball)
                    {
                        setHasFootball(false);
                        // Throw the ball, instantiate football prefab and pass it the position and direction of the player
                        GameObject newBall = (GameObject)Instantiate(Resources.Load("Football/" + equippedFootball));
                        newBall.GetComponent<FootballBehaviour>().Initialize(GetComponent<Rigidbody2D>().position, velocity);
                    }
                    else
                    {
                        timeLeftOnSpeedBurst = 0.35f;
                    }
                    actionCooldown = 0.70f;
                    // Player is checking
//                } else if (InputManager.FaceButtonRight()) {
//                    actionCooldown = 0.25f;
                }
            }
        }
        setAnimation(velocity);
        playerBody.velocity = velocity;
		// Player opens the shop screen
        if (InputManager.FaceButtonTop()){
            shopScreen.SetActive(true);
			Time.timeScale = 0f;
        }
    }

	/*
	 * Based on whether the player is moving, we need to set the parameters
	 * on the player's animator to ensure that the proper animation is shown.
	 * We also set the direction of the sprite to match the direction of movement.
	 */
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

	/*
	 * Flip the sprite over the vertical axis using localScale
	 */
	private void flipX() {
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}