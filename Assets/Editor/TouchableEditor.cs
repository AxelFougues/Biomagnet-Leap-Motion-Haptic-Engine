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
            createMaterialSettingsFoldout(touchable.onSettingsUpdated, ref touchable.matInfoFoldout, ref settingsEditor);
        }

        if (touchable.objectSettings != null) {
            EditorGUILayout.Space(20);
            createObjectSettingsFoldout(touchable.onSettingsUpdated, ref touchable.objInfoFoldout, ref settingsEditor);
        }

        EditorGUILayout.Space(10);

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

    void createObjectSettingsFoldout(System.Action onSettingsUpdated, ref bool foldout, ref Editor editor) {
        foldout = EditorGUILayout.InspectorTitlebar(foldout, touchable.objectSettings);
        using (var check = new EditorGUI.ChangeCheckScope()) {
            if (foldout) {
                CreateCachedEditor(touchable.objectSettings, null, ref editor);
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
