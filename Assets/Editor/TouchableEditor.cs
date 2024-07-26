using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Touchable))]
public class TouchableEditor : Editor{

    Touchable touchable;
    Editor settingsEditor;

    private void OnEnable() {
        touchable = (Touchable)target;
    }


    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        if (touchable.materialSettings != null) {
            EditorGUILayout.Space(20);
            createMaterialSettingsFoldout(touchable.onMaterialSettingsUpdated, ref touchable.infoFoldout, ref settingsEditor);
            EditorGUILayout.Space(10);
        }
        
    }

    void createMaterialSettingsFoldout(System.Action onSettingsUpdated, ref bool foldout, ref Editor editor) {
        foldout = EditorGUILayout.InspectorTitlebar(foldout, touchable.materialSettings);
        using (var check = new EditorGUI.ChangeCheckScope()) {
            if (foldout) {
                CreateCachedEditor(touchable.materialSettings, null, ref editor);
                editor.OnInspectorGUI();
                if (check.changed) {
                    if (onSettingsUpdated != null) {
                        onSettingsUpdated();
                    }
                }
            }
        }
    }

}
