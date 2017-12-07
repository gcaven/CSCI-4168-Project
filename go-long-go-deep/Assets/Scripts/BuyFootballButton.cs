using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Change player football and subtract gold
public class BuyFootballButton : MonoBehaviour {
    public int cost;
    public string footballType;
    public void PressButton() {
        if(Player.GetPlayer().gold >= cost){
            Player.GetPlayer().SetEquippedFootball(footballType);
            Player.GetPlayer().gold -= cost;
            //print(Player.GetPlayer().gold);
            Player.GetPlayer().goldText.text = System.Convert.ToString(Player.GetPlayer().gold);
        }
    }
}
