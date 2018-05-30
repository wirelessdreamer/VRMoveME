using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class ControllerMovement : MonoBehaviour {
    public ViveRoleProperty viveRole;
    public float MovementMultiplier = 5f;
    public CharacterController player;

    public void Move(bool MoveMe)
    {
        Vector3 movementVector = Vector3.forward * 0;
        if (MoveMe)
        {
            movementVector = Vector3.ProjectOnPlane(VivePose.GetPose(viveRole).forward, Vector3.up) * MovementMultiplier;
        }
        player.SimpleMove(movementVector); // always call simple move or you get stuck falling
    }
	// Update is called once per frame
	void Update () {
        Move(ViveInput.GetPress(viveRole, ControllerButton.Menu));
	}
}
