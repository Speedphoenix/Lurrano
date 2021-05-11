using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class InteractColoredButton : InteractButton
{
    [SerializeField] private ColorType colorName = ColorType.NoColor;

    protected override void externalAction()
    {
        ColorController.instance.enqueueNewCol(colorName);
    }
}
