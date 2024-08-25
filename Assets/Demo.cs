using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour{

    
    public Canvas floatingUI;
    public Image fill;
    public Transform cameraAnchor;
    [Space]
    public FirstPersonController controller;
    public Camera playerCamera;
    public Transform joint;
    [Space]
    public bool unlockCursor = false;


    bool playerIsNear = false;
    bool playerJoined = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            playerIsNear = true;
            floatingUI.gameObject.SetActive(true);
            fill.fillAmount = 0;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) {
            playerIsNear = false;
            floatingUI.gameObject.SetActive(false);

        }
    }


    private void Update() {
        if (!playerJoined && playerIsNear) {
            if (Input.GetKey(KeyCode.E)) {
                Vector3 viewportPoint = playerCamera.WorldToViewportPoint(floatingUI.transform.position);
                if (viewportPoint.x <= 1 && viewportPoint.x >= 0 && viewportPoint.y <= 1 && viewportPoint.y >= 0 && viewportPoint.z > 0) {
                    fill.fillAmount += Time.deltaTime;
                    if (fill.fillAmount >= 1) {
                        join();
                        fill.fillAmount = 0;
                    }
                }
            } else {
                fill.fillAmount = 0;
            }
        }

        if (playerJoined && Input.anyKeyDown) {
            if(!unlockCursor || (!Input.GetKeyDown(KeyCode.Mouse0) && !Input.GetKeyDown(KeyCode.Mouse1))) leave();
        }
    }

    void join() {
        playerJoined = true;
        floatingUI.gameObject.SetActive(false);
        controller.enabled = false;
        if(unlockCursor) Cursor.lockState = CursorLockMode.Confined;
        LeanTween.move(playerCamera.gameObject, cameraAnchor, 1f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.rotate(playerCamera.gameObject, cameraAnchor.eulerAngles, 1f).setEase(LeanTweenType.easeInOutCubic);
    }

    void leave() {
        playerJoined = false;
        floatingUI.gameObject.SetActive(true);
        controller.enabled = true;
        if (unlockCursor) Cursor.lockState = CursorLockMode.Locked;
        LeanTween.move(playerCamera.gameObject, joint, 0.5f).setEase(LeanTweenType.easeInOutCubic);
        LeanTween.rotate(playerCamera.gameObject, joint.eulerAngles, 0.5f).setEase(LeanTweenType.easeInOutCubic);

    }

}
