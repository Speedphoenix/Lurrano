using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    static PlayerMove instance = null;
    [SerializeField] private Collider mainPlayerCollider = null;
    [SerializeField] private float playerSpeed = 15f;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff = null;
    [SerializeField] private float jumpMultiplier = 8f;
    [SerializeField] private float sprintMultiplier = 1.75f;
    
    public AudioSource footsteps = null;
    [SerializeField] private float runningPitchIncrease = 1;
    private float baseAudioPitch;
    private float fastAudioPitch;

    private bool isJumping;
    private bool isSprinting;
    private bool isMoving = false;

    public static Collider MainPlayerCollider
    {
        get { return instance.mainPlayerCollider; }
    }

    void Start()
    {
        baseAudioPitch = footsteps.pitch;
        fastAudioPitch = baseAudioPitch + runningPitchIncrease;
    }

    void OnEnable()
    {
        instance = this;
        ColorController.onPause += onPause;
        ColorController.onUnPause += onUnPause;
    }

    void OnDisable()
    {
        if (instance != null)
            instance = null;
        ColorController.onPause -= onPause;
        ColorController.onUnPause -= onUnPause;
    }

    private void onPause()
    {
        footsteps.Stop();
    }

    private void onUnPause()
    {
        setAudio();
    }

    private void setAudio()
    {
        if (footsteps.isPlaying)
        {
            if (isJumping || !isMoving)
                footsteps.Stop();
            else if (footsteps.pitch == baseAudioPitch && isSprinting)
                footsteps.pitch = fastAudioPitch;
            else if (footsteps.pitch == fastAudioPitch && !isSprinting)
                footsteps.pitch = baseAudioPitch;
        }
        else
        {
            if (isJumping || !isMoving)
                return;
            else if (isSprinting)
                footsteps.pitch = fastAudioPitch;
            else
                footsteps.pitch = baseAudioPitch;
            footsteps.Play();
        }
    }

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (ColorController.instance.IsPaused)
            return;        
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = GameInputManager.getAxis(GameInputManager.Axis.Horizontal) * playerSpeed;
        float vertInput = GameInputManager.getAxis(GameInputManager.Axis.Vertical) * playerSpeed;

        Vector3 fowardMovement;
        Vector3 rightMovement;

        if (!isSprinting)
        {
            fowardMovement = transform.forward * vertInput;
            rightMovement = transform.right * horizInput;
        }
        else
        {
            fowardMovement = transform.forward * vertInput * sprintMultiplier;
            rightMovement = transform.right * horizInput * sprintMultiplier;
        }

        if (vertInput != 0 || horizInput != 0)
            isMoving = true;
        else
            isMoving = false;

        charController.SimpleMove(fowardMovement + rightMovement);

        JumpInput();
        SprintInput();
        setAudio();
    }

    private void JumpInput()
    {
        if (GameInputManager.getKeyDown(GameInputManager.InputType.Jump) && isJumping != true)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }   
    }

    private IEnumerator JumpEvent()
    {
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);

            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        isJumping = false;
    }

    private void SprintInput()
    {
        if (GameInputManager.getKeyDown(GameInputManager.InputType.Sprint) && isSprinting != true)
        {
            isSprinting = true;
        }

        if(GameInputManager.getKeyUp(GameInputManager.InputType.Sprint) && isSprinting == true)
        {
            isSprinting = false;
        }
    }

    void OnApplicationQuit()
    {
        instance = null;
    }
}
