using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(BeautyCornerCameraSwitcher))]
public class BeautyCornerCameraSwitcherEditor : Editor {
    private static GUIContent
        leftButton = new GUIContent("<-", "Left"),
        rightButton = new GUIContent("->", "Right");
    public override void OnInspectorGUI() {
        BeautyCornerCameraSwitcher myTarget = (BeautyCornerCameraSwitcher)target;

        //Switch Timer
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("switchTimer"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("freeCam"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("camMovementSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("camRotationSpeed"));
        serializedObject.ApplyModifiedProperties();

        CameraSelector(myTarget);

        //Cameras List
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("cameras"));
        serializedObject.ApplyModifiedProperties();
    }

    void CameraSelector(BeautyCornerCameraSwitcher myTarget) {
        if (myTarget.CameraCount <= 1) return;

        EditorGUILayout.BeginHorizontal();
        int camIndex = myTarget.CurrentCameraIndex;
        EditorGUILayout.PrefixLabel($"Current Camera Index " + camIndex);

        if (GUILayout.Button(leftButton)) camIndex--;
        if (GUILayout.Button(rightButton)) camIndex++;

        myTarget.curCamIndex = (camIndex + myTarget.CameraCount) % myTarget.CameraCount;
        myTarget.ChangeCam();
        EditorGUILayout.EndHorizontal();

        //Slider Version
        //myTarget.curCam = EditorGUILayout.IntSlider(myTarget.CurrentCam, 0, myTarget.CameraCount - 1);

    }
}
