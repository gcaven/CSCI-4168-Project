using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureOnPlayerEnter : MonoBehaviour {

	private float damping = 0.5f;

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
			transitionAlpha (0f);
        }
    }
    
	private void transitionAlpha(float targetAlpha) {
		foreach (Renderer renderer in gameObject.GetComponentsInChildren<Renderer>()) {
            StartCoroutine(FadeAlpha(renderer, targetAlpha, 0.5f));
		}
	}

    // https://answers.unity.com/answers/225880/view.html
    IEnumerator FadeAlpha(Renderer renderer, float targetAlpha, float fadeTime) {
        float alpha = renderer.material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeTime) {
            Color fadedColor = new Color(1, 1, 1, Mathf.Lerp(alpha, targetAlpha, ParametricBlend(t)));
            renderer.material.color = fadedColor;
            yield return null;
        }
    }

    // https://stackoverflow.com/a/25730573
    float ParametricBlend(float time) {
        float square = time*time;
        return square / (2.0f * (square - time) + 1.0f);
    }
}
    
    