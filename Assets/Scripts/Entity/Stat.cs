using System;

namespace Entity {
    public enum StatType { HEALTH, SPEED, DAMAGE, MAGIC, ATTACK_SPEED, MANA }
    [Serializable] public class Stat {
        public StatType type;
        public float value;

        public Stat(StatType type, float value) {
            this.type = type;
            this.value = value;
        }

        public override bool Equals(object obj) {
            return type == (obj as Stat).type;
        }

        public override int GetHashCode() {
            return HashCode.Combine(type);
        }

        public void Add(float amount) {
            value += amount;
        }

        public void Remove(float amount) {
            value -= amount;
        }
    }
}