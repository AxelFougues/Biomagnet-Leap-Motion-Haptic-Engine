using UnityEngine;

public class Touchable : MonoBehaviour{

    public MaterialSettings materialSettings;
    public Collider collider;

    [HideInInspector]
    public bool infoFoldout = true;

    public void onMaterialSettingsUpdated() {
        
    }

}
