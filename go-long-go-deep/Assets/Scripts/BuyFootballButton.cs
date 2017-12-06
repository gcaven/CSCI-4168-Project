using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyFootballButton : MonoBehaviour {

    public string footballType;
    public void PressButton() {
        Player.GetPlayer().SetEquippedFootball(footballType);
    }
}
