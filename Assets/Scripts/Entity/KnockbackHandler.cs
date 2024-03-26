using UnityEngine;

namespace Entity {
    public abstract class KnockbackHandler : MonoBehaviour {
        [SerializeField] protected Health health;
        [SerializeField] protected float magnitudeModifier = 0.25f;
        [SerializeField] protected Rigidbody2D rb2D;
        [SerializeField] protected LayerMask obstacles;
        [SerializeField] protected float delay = 0.5f;

        protected virtual void Start() {
            health = GetComponent<Health>();
            rb2D = GetComponent<Rigidbody2D>();
            health.onDamage += Knockback;
            obstacles = 1 << LayerMask.NameToLayer("Obstacle");
        }

        public abstract void Knockback(float damage, KnockbackData data);
    }

    public class KnockbackData {
        public Vector2 pos;
        public bool applyKnockback;

        public KnockbackData(Vector2 pos, bool applyKnockback) {
            this.pos = pos;
            this.applyKnockback = applyKnockback;
        }

        public static KnockbackData Null() {
            return new KnockbackData(Vector2.zero, false);
        }
    }
}