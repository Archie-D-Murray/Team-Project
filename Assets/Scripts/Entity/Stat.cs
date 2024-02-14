using System;

namespace Entity {
    public enum StatType { HEALTH, SPEED, DAMAGE, MAGIC, ATTACK_SPEED, MANA }
    [Serializable] public class Stat {
        public StatType type;
        public float value;

        public override bool Equals(object obj) {
            return type == (obj as Stat).type;
        }

        public override int GetHashCode() {
            return HashCode.Combine(type);
        }
    }
}