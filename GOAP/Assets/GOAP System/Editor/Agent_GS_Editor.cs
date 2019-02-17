﻿using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(Agent_GS))]
public class Agent_GS_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //Fields
        EditorGUILayout.TextField("Name", ((Agent_GS)target).GetAgentName());

        //Editor Button
        if (GUILayout.Button("Open Editor"))
        {
            //Open the editor menu in the desired path (no spaces in the path)
            EditorApplication.ExecuteMenuItem("Tools/GOAP/Node Editor");

        }

        //Remove button
        GUI.backgroundColor = new Color(1.0f, 0.2f, 0.2f, 1.0f);
        if (GUILayout.Button(/*new Rect(position.width * 0.1f, 130, position.width * 0.8f, 20),*/ "Remove Agent"))
        {
            DestroyImmediate((Agent_GS)target);
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}