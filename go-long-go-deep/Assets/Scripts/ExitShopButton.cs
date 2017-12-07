using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitShopButton : MonoBehaviour {

    public void PressButton(){
        Player.GetPlayer().GetShopScreen().SetActive(false);
    }
}
