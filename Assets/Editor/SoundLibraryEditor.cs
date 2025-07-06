using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; //For UI Toolkit (used in commented out section)
using UnityEditor; //Core Unity Editor functionalities
using UnityEditor.UIElements; //For UI Toolkit Editor controls (used in commented out section)


//CustomEditor attribute links this editor script to the 'SoundLibrary' class.
//This means whenever a 'SoundLibrary' object is selected in the Inspector,
//this script will draw its custom Inspector GUI.
//[CustomEditor(typeof (SoundLibrary))]
public class SoundLibraryEditor : Editor
{
    //public VisualTreeAsset visualTree;

    /*public override VisualElement CreateInspectorGUI()
    {
        VisualElement root = new VisualElement();

        //Add everythink from the builder to root, so it can be dravn in the inspector.
        visualTree.CloneTree(root);

        return root;
    }*/


    /*private SerializedProperty backgroundAudioClipsProp;

    private void OnEnable()
    {
        backgroundAudioClipsProp = serializedObject.FindProperty("BackgroundAudioClips");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Background Audio Clips", EditorStyles.boldLabel);

        for (int i = 0; i < backgroundAudioClipsProp.arraySize; i++)
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField($"Background Clip {i}", EditorStyles.boldLabel);

            SerializedProperty clipProp = backgroundAudioClipsProp.GetArrayElementAtIndex(i);

            // Draw the soundType (specific to BackgroundAudioClip)
            EditorGUILayout.PropertyField(clipProp.FindPropertyRelative("soundType"));

            // Draw the base class properties (ProtoAudioClip)
            EditorGUILayout.PropertyField(clipProp.FindPropertyRelative("audioClip"));

            // Draw the Setup class properties
            SerializedProperty showSetupProp = clipProp.FindPropertyRelative("showSetup");
            EditorGUILayout.PropertyField(showSetupProp, new GUIContent("Setup"));

            SerializedProperty setupProp = clipProp.FindPropertyRelative("setup"); 
            
            if (showSetupProp.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(setupProp.FindPropertyRelative("priority"));
                EditorGUILayout.PropertyField(setupProp.FindPropertyRelative("volume"));
                EditorGUILayout.PropertyField(setupProp.FindPropertyRelative("pitch"));
                EditorGUILayout.PropertyField(setupProp.FindPropertyRelative("startTime"));
                EditorGUILayout.PropertyField(setupProp.FindPropertyRelative("endTime"));


                // Draw the S3DSettings properties
                SerializedProperty show3DSettingsProp = setupProp.FindPropertyRelative("show3DSettings");
                EditorGUILayout.PropertyField(show3DSettingsProp, new GUIContent("3D Settings"));

                SerializedProperty s3dSettingsProp = setupProp.FindPropertyRelative("s3DSettings");

                if (show3DSettingsProp.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(s3dSettingsProp.FindPropertyRelative("minDistance"));
                    EditorGUILayout.PropertyField(s3dSettingsProp.FindPropertyRelative("maxDistance"));
                    EditorGUI.indentLevel--;
                }
                
                EditorGUI.indentLevel--;
            }

            if (GUILayout.Button("Remove Clip"))
            {
                backgroundAudioClipsProp.DeleteArrayElementAtIndex(i);
                break;
            }

            EditorGUILayout.EndVertical();
        }

        if (GUILayout.Button("Add New Background Clip"))
        {
            // Add a new BackgroundAudioClip to the list
            backgroundAudioClipsProp.arraySize++;
            //Debug.Log("backgroundAudioClipsProp: " + backgroundAudioClipsProp.arraySize);
            //Debug.Log("backgroundAudioClips: " + backgroundAudioClips.arraySize);

            //SerializedProperty newClipProp = backgroundAudioClipsProp.GetArrayElementAtIndex(newIndex);
            //newClipProp.FindPropertyRelative("setups").managedReferenceValue = new SoundLibrary.ProtoAudioClip.Setup();
            // You might want to initialize the 'setups' for the new clip
            /*SerializedProperty newSetupProp = newClipProp.FindPropertyRelative("setups");
            if (newSetupProp.managedReferenceValue == null)
            {
                newClipProp.FindPropertyRelative("setups").managedReferenceValue = new SoundLibrary.ProtoAudioClip.Setup();
            }/////
        }

        serializedObject.ApplyModifiedProperties();
    }*/
}
