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
    public float maxMovementMultiplier = 0.4f;
    public int yMovementLimit = 10; // number of update frames in a row moving in the same direction before we ignore movement
    public float inFrontLimitDistance = 0.5f;
    private int direction = 0;
    public SteamVR_TrackedObject tracker1;
    public SteamVR_TrackedObject tracker2;
    public SteamVR_TrackedObject rightFootObj;
    public SteamVR_TrackedObject leftFootObj;
    public SteamVR_TrackedObject HeadsetObj;

    public bool enableMotion = true;
    public string obstacleLayerName = "Obstacles"; // We don't want to be able to run though these
    public string canShootNotMoveLayerName = "canShootNotMove"; // This layer we can't run through, but can shoot through
    private int gunType;
    private int movementType;

    // Update is called once per frame
    void Update()
    {
        if (!enableMotion)
        {
            Debug.Log("VRMove Me Motion not enabled");
            return;
        }

        SteamVR_Controller.Device leftFoot;
        SteamVR_Controller.Device rightFoot;
        SteamVR_Controller.Device headset;

        rightFootObj = tracker1;
        leftFootObj = tracker2;
        

        leftFoot = SteamVR_Controller.Input((int)leftFootObj.index);
        rightFoot = SteamVR_Controller.Input((int)rightFootObj.index);
        headset = SteamVR_Controller.Input((int)HeadsetObj.index);

        // This is how we make sure to not move the player forward when going prone, or standing up
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

        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && leftFoot != null && ((leftFoot.velocity.x > 0) || (leftFoot.velocity.y > 0) || (leftFoot.velocity.z > 0)))
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
            if (canMove)
            {
                float movementMultiplier = Time.deltaTime * (Mathf.Abs(leftFoot.angularVelocity.x) * Mathf.Abs(leftFoot.angularVelocity.y) * Mathf.Abs(leftFoot.angularVelocity.z) * 1.5f);
                if (movementMultiplier > maxMovementMultiplier)
                {
                    movementMultiplier = maxMovementMultiplier;
                }
                CameraRig.transform.position += movementVector * movementMultiplier;
            }
        }

        if (enableMotion && Mathf.Abs(direction) < yMovementLimit && rightFoot != null && ((rightFoot.angularVelocity.x > 0) || (rightFoot.angularVelocity.y > 0) || (rightFoot.angularVelocity.z > 0)))
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
            if (canMove)
            {
                float movementMultiplier = Time.deltaTime * (Mathf.Abs(rightFoot.angularVelocity.x) * Mathf.Abs(rightFoot.angularVelocity.y) * Mathf.Abs(rightFoot.angularVelocity.z) * 1.5f);
                if (movementMultiplier > maxMovementMultiplier)
                {
                    movementMultiplier = maxMovementMultiplier;
                }
                CameraRig.transform.position += movementVector * movementMultiplier;
            }
        }
    }
}
