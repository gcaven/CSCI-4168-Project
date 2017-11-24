using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	private Player player;
	private Vector2 initialPos;
	private static Animator animator;
	private static Rigidbody2D skeleBody;
	public float movementSpeed = 100;
	public float attackMovementCooldown = 1.0f;
	private float attackMovementTimeout;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		animator = GetComponent<Animator>(); 
		skeleBody = GetComponent<Rigidbody2D> ();
		attackMovementTimeout = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = Player.GetPlayer ();
		}
		if (gameObject.transform.parent.gameObject == Player.GetCurrentRoom ()) {
			if (attackMovementTimeout <= 0f) {
				moveToTargetPosition (player.transform.position);
			} else {
				attackMovementTimeout -= Time.deltaTime;
				skeleBody.velocity = Vector2.zero;
			}
		} else {
			moveToTargetPosition(initialPos);
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

	void moveToTargetPosition(Vector2 targetPos) {
		Vector2 positionDiff = new Vector2 (
			(float)System.Math.Round(transform.position.x - targetPos.x,1),
			(float)System.Math.Round(transform.position.y - targetPos.y,1)
		);
		if (Mathf.Abs(positionDiff.x) <= 1.2f && Mathf.Abs(positionDiff.y) <= 1.2f) {
			Debug.Log ("attack: " + positionDiff);
			attackPlayer ();
			skeleBody.velocity = Vector2.zero;
			return;
		}
		Vector2 velocity = new Vector2 (
			Mathf.Clamp (0 - positionDiff.x, -1f, 1f) * movementSpeed * Time.deltaTime,
			Mathf.Clamp (0 - positionDiff.y, -1f, 1f) * movementSpeed * Time.deltaTime
		);
		setAnimation (velocity);
		skeleBody.velocity = velocity;
	}

	void attackPlayer() {
		animator.SetTrigger("isAttacking");
		animator.SetBool("isWalking", false);
		attackMovementTimeout = attackMovementCooldown;
	}
}
