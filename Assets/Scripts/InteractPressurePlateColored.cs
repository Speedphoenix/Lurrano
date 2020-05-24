using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class InteractPressurePlateColored : InteractPressurePlate
{
    [SerializeField] private ColorType colorName = ColorType.NoColor;

    protected override void externalAction()
    {
        ColorController.instance.setMainCol(colorName);
    }
}
