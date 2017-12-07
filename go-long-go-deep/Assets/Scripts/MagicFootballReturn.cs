using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFootballReturn : MonoBehaviour {

    private float returnTimer;

    // Set how long it takes the football to return
    void Start () {
        returnTimer = 5;
	}

	// Check if time has elapsed, if so, return football
	void Update () {
        returnTimer -= Time.deltaTime;
        if(returnTimer <= 0){
            Player.setHasFootball(true);
            Destroy(this.gameObject);
        }
    }
}
