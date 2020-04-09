﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    static PlayerMove instance = null;
    [SerializeField] private Collider playerCollider;
    [SerializeField] private string HorizontalInputName;
    [SerializeField] private string VerticalInputName;
    [SerializeField] private float playerSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private KeyCode sprintKey;
    [SerializeField] private float sprintMultiplier;
    public AudioSource footsteps;

    private bool isJumping;
    private bool isSprinting;

    public static Collider GlobalPlayerCollider
    {
        get { return instance.playerCollider; }
    }

    private void Awake()
    {
        instance = this;

        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(HorizontalInputName) * playerSpeed;
        float vertInput = Input.GetAxis(VerticalInputName) * playerSpeed;

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


        charController.SimpleMove(fowardMovement + rightMovement);

        JumpInput();
        SprintInput();
    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping == true)
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
        if (Input.GetKeyDown(sprintKey) && !isSprinting == true)
        {
            isSprinting = true;
        }

        if(Input.GetKeyUp(sprintKey) && isSprinting == true)
        {
            isSprinting = false;
        }

    }

}