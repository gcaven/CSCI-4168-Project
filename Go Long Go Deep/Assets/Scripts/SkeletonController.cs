using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	private Player player;
	private Vector2 initialPos;
	private static Animator animator;
	public float movementSpeed = 100;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
		animator = GetComponent<Animator>(); 
	}
	
	// Update is called once per frame
	void Update () {
		if (player == null) {
			player = Player.GetPlayer ();
		}
		if (gameObject.transform.parent.gameObject == Player.GetCurrentRoom ()) {
			moveToTargetPosition(player.transform.position);
		} else {
			moveToTargetPosition(initialPos);
		}
	}


	private void setAnimation(Vector2 velocity) {
		if (System.Math.Round(velocity.x, 1) != 0f || System.Math.Round(velocity.y, 1) != 0f) {
			animator.SetBool("isWalking", true);
		} else {
			animator.SetBool("isWalking", false);
		}
		if (velocity.x >= 0 && transform.localScale.x < 0) {
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
			transform.position.x - targetPos.x,
			transform.position.y - targetPos.y
		);
		Vector2 velocity = new Vector2 (
			Mathf.Clamp (1 - positionDiff.x, -1f, 1f) * movementSpeed * Time.deltaTime,
			Mathf.Clamp (1 - positionDiff.y, -1f, 1f) * movementSpeed * Time.deltaTime
		);
//		Debug.Log (velocity);
		Rigidbody2D skeleBody = GetComponent<Rigidbody2D> ();
		setAnimation (velocity);
		skeleBody.velocity = velocity;
	}
}
