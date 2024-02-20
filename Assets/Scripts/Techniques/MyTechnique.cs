using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Your implemented technique inherits the InteractionTechnique class
public class MyTechnique : InteractionTechnique
{
    [SerializeField]
    float coef = 20;

    [SerializeField]
    float precisionCoef = 5;

    [SerializeField]
    float smoothCoef = 0.2f;
    

    [SerializeField]
    float height = 0.08f;


    [SerializeField]
    private GameObject rightController;

    [SerializeField]
    private GameObject leftController;

    [SerializeField]
    private GameObject centerAnchor;

    [SerializeField]
    private GameObject debugSphereRight;

    [SerializeField]
    private GameObject debugSphereLeft;

    [SerializeField]
    private GameObject CameraRig;

    private bool OnGroundRight = false;
    private bool OnGroundLeft = false;
    private bool playerOnGround = true;

    private float baseY;

    Vector3 rightPos;
    Vector3 leftPos;

    Vector3 PrecisePositionRight;
    bool PreciseRight;

    Vector3 PrecisePositionLeft;
    bool PreciseLeft;

    private float rightCoef = 1.0f;
    private float leftCoef = 1.0f;

    private bool rightPressed = false;
    private bool leftPressed = false;

    // You must implement your technique in this file
    // You need to assign the selected Object to the currentSelectedObject variable
    // Then it will be sent through a UnityEvent to another class for handling
    private void Start()
    {
        // TODO
        baseY = CameraRig.transform.position.y;
    }

    private void Update()
    {
        //TODO : Select a GameObject and assign it to the currentSelectedObject variable

        // From torso to hand
        Vector3 toHandRight = rightController.transform.position - (centerAnchor.transform.position - height * Vector3.up);
        Vector3 HandPosRight = rightController.transform.position + rightCoef * coef * toHandRight;

        if (PreciseRight)
        {
            HandPosRight = (HandPosRight - PrecisePositionRight) * precisionCoef / (rightCoef * coef) + PrecisePositionRight;
            debugSphereRight.transform.position = HandPosRight;
        }
        else
        {
            // Smooth transition
            HandPosRight = debugSphereRight.transform.position + smoothCoef*(HandPosRight - debugSphereRight.transform.position);
            debugSphereRight.transform.position = HandPosRight;
        }

        if (playerOnGround && HandPosRight.y < 0)
        {
            OnGroundRight = true;
            OnGroundLeft = false;
            playerOnGround = false;
            rightPos = new Vector3(HandPosRight.x, 0.0f, HandPosRight.z);
        }
        if (OnGroundLeft && HandPosRight.y < 0f)
        {
            OnGroundRight = true;
            OnGroundLeft = false;
            playerOnGround = false;
            rightPos = new Vector3(HandPosRight.x, 0, HandPosRight.z);
        }

        if (OnGroundRight)
        {
            CameraRig.transform.position = CameraRig.transform.position - (HandPosRight - rightPos);
        }


        Vector3 toHandLeft = leftController.transform.position - (centerAnchor.transform.position - height * Vector3.up);
        Vector3 HandPosLeft = leftController.transform.position + leftCoef * coef * toHandLeft;

        if (PreciseLeft)
        {
            HandPosLeft = (HandPosLeft - PrecisePositionLeft) * precisionCoef / (leftCoef * coef) + PrecisePositionLeft;
            debugSphereLeft.transform.position = HandPosLeft;
        }
        else
        {
            // Smooth transition
            HandPosLeft = debugSphereLeft.transform.position + smoothCoef * (HandPosLeft - debugSphereLeft.transform.position);
            debugSphereLeft.transform.position = HandPosLeft;
        }

        if (!OnGroundLeft && HandPosLeft.y < 0)
        {
            OnGroundRight = false;
            OnGroundLeft = true;
            playerOnGround = false;
            leftPos = new Vector3(HandPosLeft.x, 0.0f, HandPosLeft.z);
        }


        if (OnGroundLeft)
        {
            CameraRig.transform.position = CameraRig.transform.position - (HandPosLeft - leftPos);
        }

        if (! playerOnGround && CameraRig.transform.position.y < baseY)
        {
            CameraRig.transform.position = new Vector3(CameraRig.transform.position.x, baseY, CameraRig.transform.position.z); ;
            OnGroundRight = false;
            OnGroundLeft = false;
            playerOnGround = true;
        }


        // Checking that the user pushed the trigger
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.1f && !rightPressed)
        {
            rightPressed = true;
            Collider[] hitColliders = Physics.OverlapSphere(debugSphereRight.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                currentSelectedObject = hitCollider.gameObject;
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) < 0.1f)
        {
            rightPressed = false;
        }

        // Checking that the user pushed the trigger
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) > 0.1f && !leftPressed)
        {
            leftPressed = true;
            Collider[] hitColliders = Physics.OverlapSphere(debugSphereLeft.transform.position, 0.1f);
            foreach (var hitCollider in hitColliders)
            {
                currentSelectedObject = hitCollider.gameObject;
            }
        }
        else if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger) < 0.1f)
        {
            leftPressed = false;
        }

        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.1f)
        {
            if (!PreciseRight)
            {
                PreciseRight = true;
                PrecisePositionRight = debugSphereRight.transform.position;
            }
        }
        else
        {
            PreciseRight = false;
        }

        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.1f)
        {
            if (!PreciseLeft)
            {
                PreciseLeft = true;
                PrecisePositionLeft = debugSphereLeft.transform.position;
            }
        }
        else
        {
            PreciseLeft = false;
        }

        if(OVRInput.Get(OVRInput.Button.One))
        {
            rightCoef = 2.0f;
        }
        else
        {
            rightCoef = 1.0f;
        }

        if (OVRInput.Get(OVRInput.Button.Three))
        {
            leftCoef = 2.0f;
        }
        else
        {
            leftCoef = 1.0f;
        }

        // DO NOT REMOVE
        // If currentSelectedObject is not null, this will send it to the TaskManager for handling
        // Then it will set currentSelectedObject back to null
        base.CheckForSelection();
    }
}
