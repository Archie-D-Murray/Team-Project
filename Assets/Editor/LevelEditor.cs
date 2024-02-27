using UnityEngine;
using UnityEditor;
using Entity.Player;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Level level = target as Level;
        if (GUILayout.Button("Add XP (10)")) {
            level.AddXP(10);
        }

        if (GUILayout.Button("Level UP")) {
            level.AddXP(level.levelXP - level.currentXP);
            Debug.Log($"Added {level.levelXP - level.currentXP} xp");
        }
    }
}