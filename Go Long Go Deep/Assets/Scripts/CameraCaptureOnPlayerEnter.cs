using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCaptureOnPlayerEnter : MonoBehaviour {
    // Use this for initialization
    void Start() {

    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(gameObject.name + ": enter");
        if (other.tag == "Player") {
            CameraMovement.target = gameObject;
            Vector2 size = GetComponent<BoxCollider2D>().size;
            float maxSize = Mathf.Max(size.x/2 + 2, size.y/2 + 2);
            CameraMovement.orthoSizeTarget = maxSize;
			Player.setCurrentRoom (gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log(gameObject.name + ": exit");
        //if (other.tag == "Player") {
            // CameraMovement.target = other.gameObject;
        //}
    }
}
    
    