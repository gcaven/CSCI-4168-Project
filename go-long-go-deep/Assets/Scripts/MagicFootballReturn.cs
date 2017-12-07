using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFootballReturn : MonoBehaviour {

    private float returnTimer;

    // Use this for initialization
    void Start () {
        returnTimer = 5;
	}
	
	// Update is called once per frame
	void Update () {
        returnTimer -= Time.deltaTime;
        if(returnTimer <= 0){
            Player.setHasFootball(true);
            Destroy(this.gameObject);
        }
    }
}
