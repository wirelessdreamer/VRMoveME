using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncCharColliderToHeadset : MonoBehaviour {
    public CharacterController CController;
    float CharacterHeightOffset = 0.69f;

	void Update () {
        CController.center = new Vector3(transform.localPosition.x, CharacterHeightOffset, transform.localPosition.z);
	}
}
