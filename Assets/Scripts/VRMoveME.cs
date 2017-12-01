using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRMoveME : MonoBehaviour {

    /*
        TODO
        Currently when a player walks through a virtual wall in the real world, they can get into place they shouldn't be, we should handle that nicely here.

    */

    public GameObject CameraRig;
    public GameObject ReverseControllerDirection;
    public float maxMovementMultiplier = 0.6f;
    public int yMovementLimit = 10; // number of update frames in a row moving in the same direction before we ignore movement
    public float TrackerCutoffThreshold = 0.02f; // This is to filter out when we are idle, and there is small movement in the tracker, so players don't move when standing still, and slightly moving
    public float inFrontLimitDistance = 0.5f;
    private int direction = 0;
    public SteamVR_TrackedObject tracker1;
    public SteamVR_TrackedObject tracker2;
    public SteamVR_TrackedObject rightFootObj;
    public SteamVR_TrackedObject leftFootObj;
    public SteamVR_TrackedObject HeadsetObj;
    SteamVR_Controller.Device leftFoot;
    SteamVR_Controller.Device rightFoot;

    public bool enableMotion = true;
    public string obstacleLayerName = "Obstacles";
    public string canShootNotMoveLayerName = "canShootNotMove";
    private int gunType;
    private int movementType;
    private bool Debug = true;
    public float RightMovementMultiplier;
    public float LeftMovementMultiplier;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!enableMotion)
        {
            //Debug.Log("VRMove Me Motion not enabled");
            return;
        }
        SteamVR_Controller.Device headset;
        
        rightFootObj = tracker1;
        leftFootObj = tracker2;

        leftFoot = SteamVR_Controller.Input((int)leftFootObj.index);
        rightFoot = SteamVR_Controller.Input((int)rightFootObj.index);
        headset = SteamVR_Controller.Input((int)HeadsetObj.index);

        if (headset.velocity.y > 0)
        {
            if (direction < 0)
            {
                direction = 0;
            }
            direction++;

        }
        else if (headset.velocity.y < 0)
        {
            if (direction > 0)
            {
                direction = 0;
            }
            direction--;
        }


        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && leftFoot != null && (Mathf.Abs(leftFoot.velocity.y) > 0))
        {
            RaycastHit hit;
            Vector3 movementVector = Vector3.ProjectOnPlane(ReverseControllerDirection.transform.up, Vector3.up);
            bool canMove = true;
            if (Physics.Raycast(ReverseControllerDirection.transform.position, movementVector, out hit, 10))
            {
                if ((hit.transform.gameObject.layer == LayerMask.NameToLayer(obstacleLayerName) || hit.transform.gameObject.layer == LayerMask.NameToLayer(canShootNotMoveLayerName)) && hit.distance < inFrontLimitDistance)
                {
                    canMove = false;
                }
            }
            if (canMove && (Mathf.Abs(leftFoot.velocity.y) > TrackerCutoffThreshold))
            {
                LeftMovementMultiplier = Time.deltaTime * Mathf.Abs(leftFoot.velocity.y) * 25f;
                if (LeftMovementMultiplier > maxMovementMultiplier)
                {
                    LeftMovementMultiplier = maxMovementMultiplier;
                }
                //Debug.Log("Movement Multiplier: " + movementMultiplier);
                CameraRig.transform.position += movementVector * LeftMovementMultiplier;
            }
        }

        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && rightFoot != null && (Mathf.Abs(rightFoot.velocity.y) > 0))
        {
            //Debug.Log("Velocity x: " + rightFoot.velocity.x + " y: " + rightFoot.velocity.y + " z: " + rightFoot.velocity.z);
            //Debug.Log("angularVelocity x: " + rightFoot.angularVelocity.x + " y: " + rightFoot.angularVelocity.y + " z: " + rightFoot.angularVelocity.z);
            RaycastHit hit;
            Vector3 movementVector = Vector3.ProjectOnPlane(ReverseControllerDirection.transform.up, Vector3.up);
            bool canMove = true;
            if (Physics.Raycast(ReverseControllerDirection.transform.position, movementVector, out hit, 10))
            {
                if ((hit.transform.gameObject.layer == LayerMask.NameToLayer(obstacleLayerName) || hit.transform.gameObject.layer == LayerMask.NameToLayer(canShootNotMoveLayerName)) && hit.distance < inFrontLimitDistance)
                {
                    canMove = false;
                }
            }
            if (canMove && (Mathf.Abs(rightFoot.velocity.y) > TrackerCutoffThreshold))
            {
                RightMovementMultiplier = Time.deltaTime * Mathf.Abs(rightFoot.velocity.y) * 25f;
                if (RightMovementMultiplier > maxMovementMultiplier)
                {
                    RightMovementMultiplier = maxMovementMultiplier;
                }
                //Debug.Log("Movement Multiplier: " + movementMultiplier);
                CameraRig.transform.position += movementVector * RightMovementMultiplier;
            }
        }
    }

    void OnGUI()
    {
        if (enableMotion && Debug)
        {
            string rightFootDisplay = "";
            string leftFootDisplay = "";

            if (rightFoot != null)
            {
                rightFootDisplay = string.Format("Right Foot - Connected:\nangular\nx [{0}]\ny [{1}]\nz [{2}]\nvel\nx [{3}]\ny [{4}]\nz [{5}]\n Multiplier[{6}]\n",
                    rightFoot.angularVelocity.x,
                    rightFoot.angularVelocity.y,
                    rightFoot.angularVelocity.z,
                    rightFoot.velocity.x,
                    rightFoot.velocity.y,
                    rightFoot.velocity.z,
                    RightMovementMultiplier
                );
            }
            else
            {
                rightFootDisplay = string.Format("Right Foot - Disconnected");
            }

            if (leftFoot != null)
            {
                leftFootDisplay = string.Format("Left Foot - Connected:\nangular\nx [{0}]\ny [{1}]\nz [{2}]\nvel\nx [{3}]\ny [{4}]\nz [{5}]\n Multiplier[{6}]\n",
                    leftFoot.angularVelocity.x,
                    leftFoot.angularVelocity.y,
                    leftFoot.angularVelocity.z,
                    leftFoot.velocity.x,
                    leftFoot.velocity.y,
                    leftFoot.velocity.z,
                    LeftMovementMultiplier
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
