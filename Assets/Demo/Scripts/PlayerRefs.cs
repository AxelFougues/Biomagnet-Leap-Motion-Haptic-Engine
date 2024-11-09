using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRefs : MonoBehaviour{

    public static PlayerRefs instance;

    public FirstPersonController controller;
    public Camera playerCamera;
    public GameObject handTracking;
    public Transform joint;
    public Transform handTrackingJoint;

    private void Awake() {
        instance = this;
    }

}
