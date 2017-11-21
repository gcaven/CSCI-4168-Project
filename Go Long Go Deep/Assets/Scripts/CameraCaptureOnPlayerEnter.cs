using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureOnPlayerEnter : MonoBehaviour {

	private float damping = 0.5f;

    void Start() {
		foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
			renderer.material.color = new Color(
				renderer.material.color.r,
				renderer.material.color.g,
				renderer.material.color.g,
				0.1f
			);
		}
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(gameObject.name + ": enter");
        if (other.tag == "Player") {
            CameraMovement.target = gameObject;
            Vector2 size = GetComponent<BoxCollider2D>().size;
            float maxSize = Mathf.Max(size.x/2 + 2, size.y/2 + 2);
            CameraMovement.orthoSizeTarget = maxSize;
			Player.setCurrentRoom (gameObject);
			transitionAlpha (1f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log(gameObject.name + ": exit");
        if (other.tag == "Player") {
			transitionAlpha (0.1f);
        }
    }

	private void transitionAlpha(float targetAlpha) {
		foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>()) {
			Material currentMaterial = renderer.material;
			Material targetMaterial = renderer.material;
			targetMaterial.color = new Color(
				currentMaterial.color.r, 
				currentMaterial.color.g,
				currentMaterial.color.b,
				targetAlpha
			);
			renderer.material.Lerp(currentMaterial, targetMaterial, Time.deltaTime * damping);
		}
	}
}
    
    