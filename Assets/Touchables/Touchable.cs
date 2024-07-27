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

    private void Start() {
        if(movableObject != null) initialPosition = movableObject.position;
    }

    private void Update() {
        if (objectSettings != null) objectSettings.update(this);
    }

    public void onSettingsUpdated() {
        
    }

}
