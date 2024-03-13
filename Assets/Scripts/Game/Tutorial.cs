using System;

using UnityEngine;

namespace Tutorial {
    [Serializable] public enum ActionType { MOVEMENT_PRESS, DASH_PRESSED, ATTACK_PRESSED, INTERACT, REWIND, INVENTORY, EQUIP_ITEM, I_FRAME }

    [CreateAssetMenu(menuName = "Tutorial")]
    public class Tutorial : ScriptableObject {
        public ActionType action;
        public string text;
    }
}