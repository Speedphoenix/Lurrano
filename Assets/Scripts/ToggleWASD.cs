using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//because you can't assign a static function on a button onclick from the editor...
public class ToggleWASD : MonoBehaviour
{
    [SerializeField] private Button useWASDButton = null;
    public void setWASD(bool value)
    {
        GameInputManager.IsWASD = value;
    }

    void Start()
    {
        // if we already use wasd (for example after coming back from the pause menu)
        if (GameInputManager.IsWASD)
            useWASDButton.onClick.Invoke();
    }
}
