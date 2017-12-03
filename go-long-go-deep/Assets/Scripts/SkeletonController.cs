using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	private Player player;
	private Vector2 initialPos;
	private Vector2 previousDirection;
	private Animator animator;
	private SpriteRenderer skeleRenderer;
	private Rigidbody2D skeleBody;
	public float movementSpeed = 100;
	public float actionCooldown = 1.0f;
	public float hitCooldown = 0.5f;
	private float hitPoints;
	private float actionTimeout;
	private bool gettingHit;
	private bool isAttacking;

	// Use this for initialization
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
	
	// Update is called once per frame
	void Update () {
		if (hitPoints <= 0) {
			return;
		}
		if (player == null) {
			player = Player.GetPlayer ();
		}
		if (gameObject.transform.parent.gameObject == Player.GetCurrentRoom ()) {
			if (actionTimeout <= 0f) {
				if (gettingHit) {
					gettingHit = false;
				}
				moveToTargetPosition (player.transform.position, true);
			} else {
				if (!gettingHit
				    && isAttacking
				    && actionTimeout <= 0.25f
				) {
					isAttacking = false;
					RaycastHit2D hit = Physics2D.Raycast (
						                   new Vector2 (transform.position.x, transform.position.y + skeleRenderer.bounds.size.y / 2), 
						                   previousDirection, 
						                   1f, 
						                   1 << 9);
					if (hit.collider != null) {
						Debug.Log ("SKELLY HIT PLAYER");
						hit.collider.gameObject.GetComponent<Player> ().takeDamage (1);
					}
				}
				actionTimeout -= Time.deltaTime;
				skeleBody.velocity = Vector2.zero;
			}
		} else {
			moveToTargetPosition(initialPos, false);
		}
	}

	// if the skeleton is hit by a moving football
	void OnTriggerEnter2D(Collider2D other) {
		if (!gettingHit 
			&& other.gameObject.tag == "Football" 
			&& other.gameObject.GetComponent<Rigidbody2D>().velocity != Vector2.zero
		) {
			registerHit ();
		}
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

	void moveToTargetPosition(Vector2 targetPos, bool attackTarget) {
		Vector2 positionDiff = new Vector2 (
			(float)System.Math.Round(transform.position.x - targetPos.x,1),
			(float)System.Math.Round(transform.position.y - targetPos.y,1)
		);
		if (attackTarget) {
			Debug.Log (positionDiff);
		}
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

	void attackPlayer() {
		animator.SetBool("isWalking", false);
		animator.SetTrigger("isAttacking");
		isAttacking = true;
		actionTimeout = actionCooldown;
	}

	void registerHit() {
		gettingHit = true;
		Debug.Log ("Hit " + Time.fixedTime);
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
