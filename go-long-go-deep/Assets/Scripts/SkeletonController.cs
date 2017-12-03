using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Script for directing Skeleton Knight enemies
 */

public class SkeletonController : MonoBehaviour {

	// private state vars
	private Player player;
	private Vector2 initialPos;
	private Vector2 previousDirection;
	private Animator animator;
	private SpriteRenderer skeleRenderer;
	private Rigidbody2D skeleBody;
	private float hitPoints;
	private float actionTimeout;
	private bool gettingHit;
	private bool isAttacking;
	// Public vars to set skeleton behaviour
	public float movementSpeed = 100;
	public float actionCooldown = 1.0f;
	public float hitCooldown = 0.5f;

	/*
	 * On initialization, store variables that will need
	 * to be referenced often. GetComponent<>(); is slow, so
	 * it is better to store these refs than call it potentially
	 * every frame.
	 * Also set starting values like health etc.
	 */ 
	void Start () {
		initialPos = transform.position;
		animator = GetComponent<Animator>(); 
		skeleRenderer = GetComponent<SpriteRenderer>();
		skeleBody = GetComponent<Rigidbody2D>();
		hitPoints = 2;
		actionTimeout = 0f;
		gettingHit = false;
		isAttacking = false;
	}

	/*
	 * Main functional loop of the Skeleton
	 * Conditionally take actions based on state
	 */
	void Update () {
		// if dead, do nothing
		if (hitPoints <= 0) {
			return;
		}
		// if do not have a reference to the player, get one.
		if (player == null) {
			player = Player.GetPlayer ();
		}
		// if the player is in the same room, navigate to them
		if (gameObject.transform.parent.gameObject == Player.GetCurrentRoom ()) {
			// if not in a cooldown, move towards the player
			if (actionTimeout <= 0f) {
				if (gettingHit) {
					gettingHit = false;
				}
				moveToTargetPosition (player.transform.position, true);
			} else {
				// If attempting attack
				if (!gettingHit
				    && isAttacking
				    && actionTimeout <= 0.25f
				) {
					checkAttackCollision();
				}
				// Skeleton cannot move during a timeout, timeout time remaining is decremented
				// every frame.
				actionTimeout -= Time.deltaTime;
				skeleBody.velocity = Vector2.zero;
			}
		// if the player is not in the same room, navigate to inital position and wait.
		} else {
			moveToTargetPosition(initialPos, false);
		}
	}

	/*
	 * If skeleton is attempting an attack, it does a raycase in the direction it last saw the player
	 * The raycast only travel for one Unity unit, and contains a layer bitmask (1<<9) 
	 * so that it only detects collisions on the "Player" layer
	 */ 
	void checkAttackCollision() {
		isAttacking = false;
		RaycastHit2D hit = Physics2D.Raycast (
			                   new Vector2 (transform.position.x, transform.position.y + skeleRenderer.bounds.size.y / 2), 
			                   previousDirection, 
			                   1f, 
			                   1 << 9);
		if (hit.collider != null) {
			// invoke damage function on the Player to decrease their health.
			hit.collider.gameObject.GetComponent<Player> ().takeDamage (1);
		}
	}

	/*
	 * If the skeleton is hit with a moving football and not a cooldown from a previous hit,
	 * then the skeleton takes a hit. The FootballBehaviour script contains the logic
	 * to stop the football from moving any further after this collision
	 */
	void OnTriggerEnter2D(Collider2D other) {
		if (!gettingHit 
			&& other.gameObject.tag == "Football" 
			&& other.gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero
		) {
			registerHit ();
		}
	}
		
	/*
	 * Based on whether the skeleton is moving, we need to set the parameters
	 * on the skeleton's animator to ensure that the proper animation is shown.
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

	/*
	 * Given a target position, the skeleton will find the difference between
	 * its current position and the target, and move smoothly towards it.
	 * Based on the attackTarget paramter, the skeleton will either reach the
	 * target and begin its idle animaton, or it will attempt to attack the target
	 */
	void moveToTargetPosition(Vector2 targetPos, bool attackTarget) {
		Vector2 positionDiff = new Vector2 (
			(float)System.Math.Round(transform.position.x - targetPos.x,1),
			(float)System.Math.Round(transform.position.y - targetPos.y,1)
		);
		if (attackTarget == true 
			&& Mathf.Abs(positionDiff.x) <= 1.1f
			&& Mathf.Abs(positionDiff.y) <= 1.1f) {
				attackPlayer ();
				skeleBody.velocity = Vector2.zero;
				return;
		}
		Vector2 velocity = new Vector2 (
			Mathf.Clamp (0 - positionDiff.x, -1f, 1f) * movementSpeed * Time.deltaTime,
			Mathf.Clamp (0 - positionDiff.y, -1f, 1f) * movementSpeed * Time.deltaTime
		);
		setAnimation (velocity);
		previousDirection = velocity;
		skeleBody.velocity = velocity;
	}

	/*
	 * When the skeleton decides to attack, it stops moving, starts the cooldown
	 * and triggers the attack collision check to occur next frame
	 */
	void attackPlayer() {
		animator.SetBool("isWalking", false);
		animator.SetTrigger("isAttacking");
		isAttacking = true;
		actionTimeout = actionCooldown;
	}

	/*
	 * When the skeleton takes damage, it stops moving
	 * If the hit kills it, we disable its physics and pull it out of the
	 * enemy layer so that nothing will collide with it, then trigger the
	 * death animation.
	 * else, play the taking damange animation and start a cooldown
	 * to delay any further actions.
	 */
	void registerHit() {
		gettingHit = true;
//		Debug.Log ("Hit " + Time.fixedTime);
		skeleBody.velocity = Vector2.zero;
		hitPoints -= 1;
		if (hitPoints <= 0) {
			animator.SetTrigger ("isDying");
			skeleBody.isKinematic = true;
			GetComponent<BoxCollider2D> ().enabled = false;
			GetComponent<SpriteRenderer>().sortingLayerName = "Floor";
		} else {
			animator.SetTrigger ("beingHit");
			animator.SetBool("isWalking", false);
			actionTimeout = hitCooldown;
		}
	}
}
