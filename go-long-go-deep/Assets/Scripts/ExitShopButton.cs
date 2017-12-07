using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Close the shop screen when button pressed
public class ExitShopButton : MonoBehaviour {
    public void PressButton(){
        Player.GetPlayer().GetShopScreen().SetActive(false);
		Time.timeScale = 1.0f;
    }
}
