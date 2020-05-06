using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ColorType = ColorController.ColorType;

public class InteractPressurePlate : MonoBehaviour
{
    [SerializeField] private ColorType colorName = ColorType.NoColor;

    private Collider playerCollider;
    private bool isInteracting;
    private bool push;
    public AudioSource pressureSoundSource;
    public AudioClip pressureSoundUp;
    [Range(0.00f, 1.00f)] public float AudioSourceVolume;

    Vector3 pushTravel = new Vector3(0.05f, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = PlayerMove.MainPlayerCollider;
        push = false;
        pressureSoundSource.volume = AudioSourceVolume;
        pressureSoundSource.spatialBlend = 1;
        isInteracting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInteracting)
        {
            externalAction();
        }
        if (isInteracting && !push)
        {
            transform.position -= pushTravel;
            push = true;
            pressureSoundSource.Play();
        } else if( !isInteracting && push)
        {
            transform.position += pushTravel;
            push = false;
            pressureSoundSource.PlayOneShot(pressureSoundUp);
        }
    }

    // this could be overridden by different pressure plates
    protected virtual void externalAction()
    {
        ColorController.instance.setMainCol(colorName);
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        if (otherCollider != playerCollider)
            return;
        isInteracting = true;
    }

    void OnTriggerExit(Collider otherCollider)
    {
        if (otherCollider != playerCollider)
            return;
        isInteracting = false;
    }
}
