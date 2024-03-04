
using UnityEditor;
using UnityEngine;
using Entity.Player;
using System;

[CustomPropertyDrawer(typeof(SwordAnimator))]
public class SwordAnimatorDrawer : WeaponAnimatorDrawer {
    private const int FIELD_COUNT = 6;

    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        string[] fields = new string[6] {
            "allowMouseRotation",
            "radius",
            "rotationSpeed",
            "swingDirection",
            "swingRotation",
            "angleOffset",
        };

        DrawFields(fields, position, property, label);
    }

    

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * FIELD_COUNT + PADDING * (FIELD_COUNT - 1);
    }
}