using UnityEditor;
using UnityEngine;
using Data;

[CustomEditor(typeof(SaveManager))]
public class SaveManagerEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        SaveManager manager = target as SaveManager;
        if (GUILayout.Button("Save")) {
            manager.Save();
        }

        if (GUILayout.Button("Load")) {
            manager.Load();
        }
    }
}