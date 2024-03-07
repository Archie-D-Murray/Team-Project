using UnityEditor;
using UnityEngine;
using Entity.Player;
using System;

[CustomPropertyDrawer(typeof(WeaponAnimator))]
public class WeaponAnimatorDrawer : PropertyDrawer {
    protected const int PADDING = 5;
    private const int FIELD_COUNT = 3;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        DrawFields(
            new string[FIELD_COUNT] { "allowMouseRotation", "radius", "rotationSpeed" },
            position,
            property,
            label
        );
    }

    public void DrawFields(string[] fields, Rect position, SerializedProperty property, GUIContent label) {
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.y += position.height / (FIELD_COUNT * FIELD_COUNT);
        EditorGUI.indentLevel = 0;
        position.x = 25;
        try {
            _ = property.FindPropertyRelative("allowMouseRotation").boolValue;
        } catch (NullReferenceException) {
            return;
        }
        for (int i = 0; i < fields.Length; i++) {
            EditorGUI.BeginProperty(position, label, property);
            DrawPropertySafe(
                new Rect(position.x, position.y, position.width, (position.height / (fields.Length * fields.Length)) + 2 * PADDING),
                fields[i],
                property,
                null
            );
            EditorGUI.EndProperty();
            position.y += (position.height / (fields.Length * fields.Length)) + PADDING * PADDING;
        }
    }

    public void DrawPropertySafe(Rect position, string name, SerializedProperty property, GUIContent label) {
        try {
            EditorGUI.PropertyField(position, property.FindPropertyRelative(name), label);
        } catch (NullReferenceException) {
            position.x += 40;
            EditorGUI.HelpBox(position, $"Currently null: {name}", MessageType.Warning);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return 0.5f * base.GetPropertyHeight(property, label) * FIELD_COUNT + (PADDING * (FIELD_COUNT - 1));
    }
}