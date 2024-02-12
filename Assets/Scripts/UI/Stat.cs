using System;

namespace UI {
    public partial class Stats {
        [Serializable]
        public class Stat {
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
}