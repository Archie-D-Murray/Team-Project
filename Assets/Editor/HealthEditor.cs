using Entity;

using UnityEditor;

using UnityEngine;

[CustomEditor(typeof(Health))]
public class HealthEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Health health = target as Health;
        if (GUILayout.Button("Damage (10)")) {
            if (Application.isPlaying) {
                health.Damage(10f);
            } else {
                Debug.Log("Make sure you are in play mode to use this!");
            }
        }
        if (GUILayout.Button("Heal (10)")) {
            if (Application.isPlaying) {
                health.Heal(10);
            } else {
                Debug.Log("Make sure you are in play mode to use this!");
            }
        }
    }
}