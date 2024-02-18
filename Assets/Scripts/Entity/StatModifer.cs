using UnityEngine;

namespace Entity {
    public class StatModifier {
        public float value;
        public float duration;
        public bool isFinished => duration == 0.0f;

        public StatModifier(float amount, float duration) { 
            this.value = amount;
            this.duration = duration;
        }

        public void Update(float delta) {
            duration = Mathf.Max(0.0f, duration - delta);
        }
    }
}