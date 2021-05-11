using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractReverserButton : InteractButton
{
    public enum ActionOnHourglass
    {
        Toggle, SetQueue, SetStack
    }
    [SerializeField] private ActionOnHourglass action = ActionOnHourglass.Toggle;

    protected override void externalAction()
    {
        switch (action)
        {
            case ActionOnHourglass.Toggle:
                ColorController.instance.toggleHourglassType();
                break;
            case ActionOnHourglass.SetQueue:
                ColorController.instance.IsStack = false;
                break;
            case ActionOnHourglass.SetStack:
                ColorController.instance.IsStack = true;
                break;
            default:
                break;
        }
    }
}
