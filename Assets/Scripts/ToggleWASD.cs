using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//because you can't assign a static function on a button onclick from the editor...
public class ToggleWASD : MonoBehaviour
{
    public void setWASD(bool value)
    {
        GameInputManager.IsWASD = value;
    }
}
