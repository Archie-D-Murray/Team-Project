using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(UI.Stats.Stat))]
public class StatDrawer : PropertyDrawer {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        EditorGUI.BeginProperty (position, label, property);

        int padding = 50;

        // Draw label
        position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

        // Don't make child fields be indented
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        Rect statTypeRect = new Rect(position.x, position.y, 200, position.height);
        Rect statValueRect = new Rect(position.x + statTypeRect.width + padding, position.y, 50, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField (statTypeRect, property.FindPropertyRelative ("type"), GUIContent.none);
        EditorGUI.PropertyField (statValueRect, property.FindPropertyRelative ("value"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty ();
    }
}