using UnityEngine;

namespace Entity {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnockbackHandler : MonoBehaviour {
        [SerializeField] private Health health;
        [SerializeField] private Rigidbody2D rb2D;

        private void Start() {
            health = GetComponent<Health>();
            rb2D = GetComponent<Rigidbody2D>();
            health.onDamage += Knockback;
        }

        public void Knockback(float damage, KnockbackData data) {
            if (data.applyKnockback) {
                rb2D.MovePosition(rb2D.position + (rb2D.position - data.pos).normalized * damage);
            }
        }
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