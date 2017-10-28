using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // A button on Xbox, X button on Dualshock
    public static bool FaceButtonBottom() {
        return Input.GetButtonDown("faceButtonBottom");
    }

    // Y button on Xbox, Triangle button on Dualshock
    public static bool FaceButtonTop() {
        return Input.GetButtonDown("faceButtonTop");
    }

    // X button on Xbox, Square button on Dualshock
    public static bool FaceButtonLeft() {
        return Input.GetButtonDown("faceButtonLeft");
    }

    // B button on Xbox, circle button on Dualshock
    public static bool FaceButtonRight() {
        return Input.GetButtonDown("faceButtonRight");
    }

}
