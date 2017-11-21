using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour {

	private Player player;
	private Vector2 initialPos;
	public float movementSpeed = 100;

	// Use this for initialization
	void Start () {
		initialPos = transform.position;
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
		skeleBody.velocity = velocity;
	}
}
