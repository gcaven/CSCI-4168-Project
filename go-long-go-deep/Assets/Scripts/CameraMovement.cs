using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public static GameObject target;
    public static float orthoSizeTarget = 6;

    public GameObject player;
    public Camera camera;
    public float damping = 0.5f;
    Vector3 offset;

    // On start, get distance between target and camera
    // Camera will attempt to keep this distance
    void Start () {
        target = player;
        offset = transform.position - target.transform.position;
	}

    // Use lerp to smoothly move camera between current position and the new player position
    // damping keeps the camera a little bit behind, makes the movement feel smoother
    void LateUpdate () {
        Vector3 desiredPosition = target.transform.position + offset;
        Vector3 position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * damping);
        transform.position = position;
        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, orthoSizeTarget, Time.deltaTime * damping);
	}
}
