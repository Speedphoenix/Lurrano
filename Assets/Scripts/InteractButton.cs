using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class InteractButton : MonoBehaviour
{

    // [SerializeField] private string facingDirection;
    [Range(0.00f, 1.00f)] public float AudioSourceVolume = 0.05F;
    [SerializeField] private int timerDuration = 35;
    public GameObject button = null;
    public AudioSource pressureSoundSource;
    private double pushedDistance = 0.15;
    private bool isInteracting;
    private bool push;
    private int timer;

    Vector3 pushTravel = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        // StartFacing();
        pressureSoundSource.volume = AudioSourceVolume;
        pressureSoundSource.spatialBlend = 1;
        isInteracting = false;
        push = false;
        timer = 0;
        float dimension = transform.localScale.x;
        pushTravel = new Vector3(0 , (float)pushedDistance * dimension, 0);

    }

    /*
    void StartFacing()
    {
        switch (facingDirection)
        {
            case "XP":
                break;
            case "XN":
                break;
            case "ZP":
                break;
            case "ZN":
                break;
            default:
                break;
        }
    }
    */

    // Update is called once per frame
    void Update()
    {
        if (isInteracting && !push)
        {
            externalAction();
            pressureSoundSource.Play();
            button.transform.localScale -= pushTravel;
            push = true;
            timer = timerDuration;
        }
        if(!isInteracting && push && timer == 0)
        {
            button.transform.localScale += pushTravel;
            push = false;
        }
    }

    void FixedUpdate()
    {
        if (timer > 0)
            timer--;
    }

    // this will be overridden by other buttons
    protected virtual void externalAction()
    {
    }

    void OnTriggerStay(Collider otherCollider)
    {
        if (otherCollider.gameObject.tag != "Player")
            return;

        if (GameInputManager.getKeyDown(GameInputManager.InputType.Interact))
        {
            isInteracting = true;
        }
        else
        {
            isInteracting = false;
        }
    }

}
