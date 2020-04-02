using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class lightCol : MonoBehaviour
{
    private Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
    }

    private void OnEnable()
    {
        ColorController.onColChange += onColorChange;
    }

    private void OnDisable()
    {
        ColorController.onColChange -= onColorChange;
    }

    private void onColorChange(ColorType newCol)
    {
        lt = GetComponent<Light>();
        // tu mets ici le code pour quand la couleur change
        Color inter = ColorController.getColorFromType(newCol);
        lt.color = inter;

    }
}
