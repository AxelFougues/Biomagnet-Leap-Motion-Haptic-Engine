using System.Collections.Generic;
using UnityEngine;

public class Touchable : MonoBehaviour{

    [Header("Settings")]
    [Space]
    
    public MaterialSettings materialSettings;
    public ObjectSettings objectSettings;

    [Space]
    [Header("References")]
    [Space]

    public Transform movableObject;
    public Collider touchCollider;



    [HideInInspector]
    public bool matInfoFoldout = true;
    [HideInInspector]
    public bool objInfoFoldout = true;

    [HideInInspector]
    public Vector3 initialPosition = Vector3.zero;

    public bool inPlace = true;

    public int inContact = 0;

    private void Start() {
        if(movableObject != null) initialPosition = movableObject.position;
    }

    private void Update() {
        if (objectSettings != null && inContact == 0) objectSettings.doRelease(this);
    }

    public void onContactStart() {
        inContact++;
    }

    public void onContactEnd() {
        inContact--;
    }

    public void onSettingsUpdated() {
        
    }

}
