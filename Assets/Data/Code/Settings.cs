using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class InputSettings
{
    //key bindings
    public static KeyCode InteractKey = KeyCode.Mouse0;
    public static KeyCode ItemKey = KeyCode.Mouse1;
    public static KeyCode CloseDoorKey = KeyCode.Space;
    public static KeyCode SwipeLeftKey = KeyCode.A;
    public static KeyCode SwipeRightKey = KeyCode.D;

    //helper methods
    public static bool interact() => Input.GetKeyDown(InteractKey);
    public static bool item() => Input.GetKeyDown(ItemKey);
    public static bool CloseDoor() => Input.GetKeyDown(CloseDoorKey);
    public static bool SwipeLeft() => Input.GetKeyDown(SwipeLeftKey);
    public static bool SwipeRight() => Input.GetKeyDown(SwipeRightKey);

}

public class Setters
{
    public enum Getter
    {
        Interact,
        Item,
        CloseDoor,
        SwipeLeft,
        SwipeRight
    }

    public void ResetInputSettings()
    {
        InputSettings.InteractKey = KeyCode.Mouse0;
        InputSettings.ItemKey = KeyCode.Mouse1;
        InputSettings.CloseDoorKey = KeyCode.Space;
        InputSettings.SwipeLeftKey = KeyCode.A;
        InputSettings.SwipeRightKey = KeyCode.D;
    }

    public void SetKey(Getter input, KeyCode newKey)
    {
        switch (input)
        {
            case Getter.Interact:
                InputSettings.InteractKey = newKey;
                break;
            case Getter.Item:
                InputSettings.ItemKey = newKey;
                break;
            case Getter.CloseDoor:
                InputSettings.CloseDoorKey = newKey;
                break;
            case Getter.SwipeLeft:
                InputSettings.SwipeLeftKey = newKey;
                break;
            case Getter.SwipeRight:
                InputSettings.SwipeRightKey = newKey;
                break;
        }
    }
}