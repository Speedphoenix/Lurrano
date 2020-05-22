using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonTriggerer : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private Transform cameraTransform = null;
    

    private static GameObject currentlyInView = null;

    public static GameObject CurrentlyInView
    {
        get { return currentlyInView; }
    }

    void Update()
    {
        RaycastHit hit;
        Ray interactRay = new Ray(cameraTransform.position, cameraTransform.forward);
    
        Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance);

        if (Physics.Raycast(interactRay, out hit, interactionDistance))
        {
            currentlyInView = hit.collider.gameObject;
            Debug.Log(hit.collider.gameObject.name);
        }
        else
            currentlyInView = null;

    }
}
