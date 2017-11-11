using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    static GameObject player;

    private static bool hasFootball;

	// Use this for initialization
	void Start () {
        hasFootball = true;
        player = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        
	}

    public static bool getHasFootball()
    {
        return hasFootball;
    }
    public static void setHasFootball(bool val)
    {
        hasFootball = val;
    }

    public static GameObject getPlayer()
    {
        if (!player)
        {
            player = GameObject.Find("Player");
        }
        return player;
        
    }
}
