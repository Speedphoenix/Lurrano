using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName = "Mouse X";
    [SerializeField] private string mouseYInputName = "Mouse y";
    [SerializeField] private float mouseSensitivity = 2f;

    [SerializeField] private Transform playerBody = null;

    private float XAxisClamp;

    void Start()
    {
        // because WebGL has a bad mouse sensitivity...
        #if UNITY_WEBGL
            mouseSensitivity /= 2;
        #endif
    }

    private void OnEnable()
    {
        LockCursor();
        XAxisClamp = 0.0f;
        ColorController.onPause += UnlockCursor;
        ColorController.onUnPause += LockCursor;
    }

    private void OnDisable()
    {
        UnlockCursor();
        ColorController.onPause -= UnlockCursor;
        ColorController.onUnPause -= LockCursor;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (ColorController.instance.IsPaused)
            return;
        CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseX = Input.GetAxis(mouseXInputName) * mouseSensitivity;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseSensitivity;

        
        XAxisClamp += mouseY;

        
        if(XAxisClamp > 90.0f)
        {
            XAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }

        if (XAxisClamp < -90.0f)
        {
            XAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }
        

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);

    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }


}
