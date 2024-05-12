using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Entity.Player.PlayerController))]
public class PlayerControllerEditor : Editor {
    public override void OnInspectorGUI() {
        Entity.Player.PlayerController controller = target as Entity.Player.PlayerController;
        if (GUILayout.Button("Debug Initialise")) {
            controller.DebugInitialise();
        }
        DrawDefaultInspector();
    }
}