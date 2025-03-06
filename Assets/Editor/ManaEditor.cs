using Entity;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Mana))]
public class ManaEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Mana mana = target as Mana;
        if (GUILayout.Button("Use Mana (10)")) {
            if (Application.isPlaying) {
                mana.UseMana(10);
            } else {
                Debug.Log("Make sure you are in play mode to use this!");
            }
        }
    }
}