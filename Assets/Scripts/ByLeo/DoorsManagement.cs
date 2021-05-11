using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class DoorsManagement : MonoBehaviour
{
    public Animator animator;
    protected bool doorOpen = false;
    [SerializeField] GameObject doorCollider = null;
    
    public ColorType color;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        ColorController.onColChange += onColorChange;
    }

    private void OnDisable()
    {
        ColorController.onColChange -= onColorChange;
    }

    void doors (string direction)
    {
        animator.SetTrigger(direction);
    }

    private void onColorChange(ColorType newCol)
    {
        if(newCol == color && !doorOpen)
        {
            doorOpen = true;
            doors("open");
            doorCollider.SetActive(false);
        }
        else if(newCol != color && doorOpen)
        {
            doorOpen = false;
            doors("close");
            doorCollider.SetActive(true);
        }
    }
}
