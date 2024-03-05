using Entity.Player;

using UnityEditor;

using UnityEngine;

[CustomPropertyDrawer(typeof(MageStaffAnimator))]
public class MageStaffAnimatorDrawer : WeaponAnimatorDrawer {
    private const int FIELD_COUNT = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        string[] fields = new string[FIELD_COUNT] {
            "allowMouseRotation",
            "radius",
            "rotationSpeed",
            "angleOffset",
        };

        DrawFields(fields, position, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return base.GetPropertyHeight(property, label) * FIELD_COUNT + PADDING * (FIELD_COUNT - 1);
    }
}