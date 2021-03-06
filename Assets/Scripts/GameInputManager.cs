﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInputManager : MonoBehaviour
{
    static private GameInputManager instance = null;

    public enum InputType
    {
        Interact, Jump, Sprint, AccelerateTime, Pause
    }

    public enum Axis
    {
        Vertical, Horizontal
    }

    [SerializeField] private string HorizontalInputName = "Horizontal";
    [SerializeField] private string VerticalInputName = "Vertical";
    [SerializeField] private string WASDHorizontalInputName = "HorizontalWASD";
    [SerializeField] private string WASDVerticalInputName = "VerticalWASD";
    [SerializeField] private static bool isWASD = false;

    // These are put one by one to make it easier in the editor.
    // use KeyCode.None to disable one of these
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode accelerateKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode pauseKey = KeyCode.P;
    [SerializeField] private KeyCode pauseKey2 = KeyCode.Escape;    

    public static bool IsWASD {
        get
        {
            return isWASD;
        }

        set
        {
            isWASD = value;
        }
    }

    private KeyCode getKey(InputType which)
    {
        switch (which)
        {
            case InputType.Interact:
                return interactKey;

            case InputType.Jump:
                return jumpKey;

            case InputType.Sprint:
                return sprintKey;

            case InputType.AccelerateTime:
                return accelerateKey;

            case InputType.Pause:
                return pauseKey;
            
            default:
                return KeyCode.None;
        }
    }

    public static bool getKeyDown(InputType which)
    {
        if (which == InputType.Pause)
            return Input.GetKeyDown(instance.getKey(which)) || Input.GetKeyDown(instance.pauseKey2);
        return Input.GetKeyDown(instance.getKey(which));
    }

    public static bool getKeyUp(InputType which)
    {
        if (which == InputType.Pause)
            return Input.GetKeyUp(instance.getKey(which)) || Input.GetKeyUp(instance.pauseKey2);
        return Input.GetKeyUp(instance.getKey(which));
    }

    public static float getAxis(Axis which)
    {
        string inputName = instance.VerticalInputName;
        switch (which)
        {
            case Axis.Vertical:
                if (!isWASD)
                    inputName = instance.VerticalInputName;
                else
                    inputName = instance.WASDVerticalInputName;
                break;

            case Axis.Horizontal:
                if (!isWASD)
                    inputName = instance.HorizontalInputName;
                else
                    inputName = instance.WASDHorizontalInputName;
                break;

            default:
                break;
        }
        return Input.GetAxis(inputName);
    }

    void OnEnable()
    {
        if (instance != null && instance != this)
            Application.Quit(); // replace this with a proper throw statement
        else
            instance = this;
    }

    void OnDisable()
    {
        if (instance != null)
            instance = null;
    }

    void OnApplicationQuit()
    {
        instance = null;
    }
}
