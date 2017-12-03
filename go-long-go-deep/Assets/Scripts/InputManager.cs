using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This script manages input from Keyboard & Gamepad,
 * allowing players to switch seamlessly between the two
 * and allowing scripts to reference a single function to determine
 * if a certain button as been pressed, rather than duplicate
 * conditionals checking both separately. To change the mappings
 * go to Edit -> Project Settings -> Input, but be sure to
 * change the comments in here to be consistant.
 */ 

public static class InputManager {

    // Primary Axis -- Left Joystick/WASD
    public static float PrimaryAxisHorizontal() {
        float axisValue = 0.0f;
        axisValue += Input.GetAxis("LeftJoystick_Horizontal");
        axisValue += Input.GetAxis("KB_Horizontal");
        return Mathf.Clamp(axisValue, -1f, 1f);
    }

    public static float PrimaryAxisVertical() {
        float axisValue = 0.0f;
        axisValue += Input.GetAxis("LeftJoystick_Vertical");
        axisValue += Input.GetAxis("KB_Vertical");
        return Mathf.Clamp(axisValue, -1f, 1f);
    }

    public static Vector2 PrimaryAxis() {
        return new Vector2(PrimaryAxisHorizontal(), PrimaryAxisVertical());
    }

    // Face Buttons (A,B,X,Y)

    // A button on Xbox, X button on Dualshock, E on Keyboard
    public static bool FaceButtonBottom() {
        return Input.GetButtonDown("faceButtonBottom");
    }

    // Y button on Xbox, Triangle button on Dualshock, Q on Keyboard
    public static bool FaceButtonTop() {
        return Input.GetButtonDown("faceButtonTop");
    }

    // X button on Xbox, Square button on Dualshock, Z on Keyboard
    public static bool FaceButtonLeft() {
        return Input.GetButtonDown("faceButtonLeft");
    }

    // B button on Xbox, circle button on Dualshock, X on Keyboard
    public static bool FaceButtonRight() {
        return Input.GetButtonDown("faceButtonRight");
    }

}
