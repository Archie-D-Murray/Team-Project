using Entity.Player;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(RangedAnimator))]
public class RangedAnimatorDrawer : WeaponAnimatorDrawer {
    private const int FIELD_COUNT = 5;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        string[] fields = new string[FIELD_COUNT] {
            "allowMouseRotation",
            "radius",
            "rotationSpeed",
            "renderer",
            "frames",
        };

        DrawFields(fields, position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * FIELD_COUNT + PADDING * (FIELD_COUNT - 1);
    }
}