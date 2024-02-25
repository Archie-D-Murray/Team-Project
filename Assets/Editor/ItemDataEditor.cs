using Items;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(ItemData))]
public class ItemDataEditor : Editor {
    public override void OnInspectorGUI() {
        ItemData data = target as ItemData;
        if (GUILayout.Button("Create ID")) {
            data.id = HashCode.Combine(data.GetType().ToString(), data.itemName);
        }
        DrawDefaultInspector();
    }
}
[CustomEditor(typeof(BowData))]
public class BowDataEditor : ItemDataEditor {}

[CustomEditor(typeof(ConsumableData))]
public class ConsumableDataEditor : ItemDataEditor {}

[CustomEditor(typeof(SwordData))]
public class SwordDataEditor : ItemDataEditor {}