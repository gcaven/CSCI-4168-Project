using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Attach this script to the base GameObject of a dungeon room
 * NOTE:
 * The base object should be constructed so that the transform on the
 * base object is at the center of the room
 * All child object should be position relative to this center
 * The room should have a trigger covering the entire navigable surface
 * of the room.
 * 
 * When the playe enters a room with this script, the game camera moves
 * to the center of the room. The camera's orthographic size is adjusted
 * so that entire room fits in the frame. Rooms are completely transparent
 * by default, but when the player enters one it will fade to visbility
 * and vice-versa.
 */

public class CameraCaptureOnPlayerEnter : MonoBehaviour {

	/* 
	 * All rooms other than the initial room start faded out and non-visible.
	 * This loops through all children of the room and modifies their active
	 * material's alpha to make them completely transparent. This could
	 * probably be done more efficiently as a shader, but we did not have
	 * time to learn Unity shaders, and this works fine.
	 */
	void Start() {
        if (gameObject.name != "Room1") {
            foreach (Renderer renderer in GetComponentsInChildren<Renderer>()) {
                renderer.material.color = new Color(
                    renderer.material.color.r,
                    renderer.material.color.g,
                    renderer.material.color.g,
                    0f
                );
            }
        }
    }

	/*
	 * Trigger logic to check for player entry.
	 * Finds the size and position of the room and adjust the camera
	 * perspective accordingly.
	 */ 
    private void OnTriggerEnter2D(Collider2D other) {
//        Debug.Log(gameObject.name + ": enter");
        if (other.tag == "Player") {
            CameraMovement.target = gameObject;
            Vector2 size = GetComponent<BoxCollider2D>().size;
            float maxSize = Mathf.Max(size.x/2 + 2, size.y/2 + 2);
            CameraMovement.orthoSizeTarget = maxSize;
			Player.setCurrentRoom (gameObject);
			transitionAlpha (1f);
        }
    }

	/*
	 * When the player leaves, fade room to black
	 */
    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log(gameObject.name + ": exit");
        if (other.tag == "Player") {
			transitionAlpha (0f);
        }
    }
    
	/*
	 * Starts a coroutine to run FadeAlpha
	 */ 
	private void transitionAlpha(float targetAlpha) {
		foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>()) {
            StartCoroutine(FadeAlpha(renderer, targetAlpha, 0.5f));
		}
	}
		
	/*
	 * Blend renderer material alpha from opaque to transparent or vice-versa
	 * Credit: https://answers.unity.com/answers/225880/view.html
	 */ 
    IEnumerator FadeAlpha(Renderer renderer, float targetAlpha, float fadeTime) {
        float alpha = renderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime) {
            Color fadedColor = new Color(1, 1, 1, Mathf.Lerp(alpha, targetAlpha, ParametricBlend(t)));
            renderer.material.color = fadedColor;
            yield return null;
        }
    }

	/*
	 * Add smoothing to a transition, since linear transitions look bad
	 * Uses a parametric blending technique on a timescale from 0 to 1
	 * Credit: https://stackoverflow.com/a/25730573
	 */ 
    float ParametricBlend(float time) {
        float square = time*time;
        return square / (2.0f * (square - time) + 1.0f);
    }
}
    
    