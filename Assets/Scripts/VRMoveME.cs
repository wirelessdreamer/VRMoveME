using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

[RequireComponent(typeof(CharacterController))]
public class VRMoveME : MonoBehaviour {

    public GameObject ReverseControllerDirection;
    public float minMovementMultiplier = 0.05f;
    public float maxMovementMultiplier = 0.6f;
    public float movementMultiplier = 0;
    public float movementScale = 15;
    public int yMovementLimit = 10; // number of update frames in a row moving in the same direction before we ignore movement
    private int direction = 0;
    public ViveRoleProperty rightFoot;
    public ViveRoleProperty leftFoot;
    public ViveRoleProperty headSet;

    public float LeftAngularVelocity;
    public float LeftVelocity;
    public float RightAngularVelocity;
    public float RightVelocity;

    public bool enableMotion = true;
    private int movementType;
    CharacterController player;

    public bool ShowDebug = false;

    // Use this for initialization
    void Awake()
    {
        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!enableMotion)
        {
            return;
        }

        if (VivePose.GetVelocity(headSet).y > 0)
        {
            if (direction < 0)
            {
                direction = 0;
            }
            direction++;

        }
        else if (VivePose.GetVelocity(headSet).y < 0)
        {
            if (direction > 0)
            {
                direction = 0;
            }
            direction--;
        }

        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && leftFoot != null)
        {
            LeftAngularVelocity = Mathf.Abs(VivePose.GetAngularVelocity(leftFoot).x) + Mathf.Abs(VivePose.GetAngularVelocity(leftFoot).y) + Mathf.Abs(VivePose.GetAngularVelocity(leftFoot).z);
            LeftVelocity = Mathf.Abs(VivePose.GetVelocity(leftFoot).x) + Mathf.Abs(VivePose.GetVelocity(leftFoot).y) + Mathf.Abs(VivePose.GetVelocity(leftFoot).z);
            Vector3 movementVector = Vector3.ProjectOnPlane(ReverseControllerDirection.transform.up, Vector3.up);
            movementMultiplier = Time.deltaTime * (LeftAngularVelocity + LeftVelocity);
            if (movementMultiplier > maxMovementMultiplier)
            {
                movementMultiplier = maxMovementMultiplier;
            }else if (movementMultiplier < minMovementMultiplier)
            {
                movementMultiplier = 0f;
            }

            Vector3 MovementLeftAmount = movementVector * movementMultiplier * movementScale;
           
            player.SimpleMove(MovementLeftAmount);
        }

        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && rightFoot != null)
        {
            RightAngularVelocity = Mathf.Abs(VivePose.GetAngularVelocity(rightFoot).x) + Mathf.Abs(VivePose.GetAngularVelocity(rightFoot).y) + Mathf.Abs(VivePose.GetAngularVelocity(rightFoot).z);
            RightVelocity = Mathf.Abs(VivePose.GetVelocity(rightFoot).x) + Mathf.Abs(VivePose.GetVelocity(rightFoot).y) + Mathf.Abs(VivePose.GetVelocity(rightFoot).z);

            Vector3 movementVector = Vector3.ProjectOnPlane(ReverseControllerDirection.transform.up, Vector3.up);
            
            movementMultiplier = Time.deltaTime * (RightAngularVelocity + RightVelocity);
            if (movementMultiplier > maxMovementMultiplier)
            {
                movementMultiplier = maxMovementMultiplier;
            }
            else if (movementMultiplier < minMovementMultiplier)
            {
                movementMultiplier = 0f;
            }
            Vector3 MovementRightAmount = movementVector * movementMultiplier * movementScale;

            player.SimpleMove(MovementRightAmount);
        }
        player.SimpleMove(new Vector3(0, 0, 0));
    }

    void OnGUI()
    {
        if (enableMotion && ShowDebug)
        {
            string rightFootDisplay = "";
            string leftFootDisplay = "";

            if (rightFoot != null)
            {
                rightFootDisplay = string.Format("Right Foot - Connected:\nangular\nx [{0}]\ny [{1}]\nz [{2}]\nvel\nx [{3}]\ny [{4}]\nz [{5}]\n",
                    VivePose.GetAngularVelocity(rightFoot).x,
                    VivePose.GetAngularVelocity(rightFoot).y,
                    VivePose.GetAngularVelocity(rightFoot).z,
                    VivePose.GetVelocity(rightFoot).x,
                    VivePose.GetVelocity(rightFoot).y,
                    VivePose.GetVelocity(rightFoot).z
                );
            }
            else
            {
                rightFootDisplay = string.Format("Right Foot - Disconnected");
            }

            if (leftFoot != null)
            {
                leftFootDisplay = string.Format("Left Foot - Connected:\nangular\nx [{0}]\ny [{1}]\nz [{2}]\nvel\nx [{3}]\ny [{4}]\nz [{5}]\n",
                    VivePose.GetAngularVelocity(leftFoot).x,
                    VivePose.GetAngularVelocity(leftFoot).y,
                    VivePose.GetAngularVelocity(leftFoot).z,
                    VivePose.GetVelocity(leftFoot).x,
                    VivePose.GetVelocity(leftFoot).y,
                    VivePose.GetVelocity(leftFoot).z
                );
            }
            else
            {
                rightFootDisplay = string.Format("Left Foot - Disconnected");
            }

            GUI.Label(new Rect(10, 10, 500, 300), rightFootDisplay);
            GUI.Label(new Rect(10, 350, 500, 300), leftFootDisplay);
        }
    }
}
