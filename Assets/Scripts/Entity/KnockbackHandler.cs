using UnityEngine;

namespace Entity {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnockbackHandler : MonoBehaviour {
        [SerializeField] private Health health;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private LayerMask obstacles;

        Ray2D hit;

        private void Start() {
            health = GetComponent<Health>();
            rb2D = GetComponent<Rigidbody2D>();
            health.onDamage += Knockback;
            obstacles = 1 << LayerMask.NameToLayer("Obstacle");
        }

        public void Knockback(float damage, KnockbackData data) {
            if (data.applyKnockback) {
                rb2D.MovePosition(Physics2D.Raycast(rb2D.position, (data.pos - rb2D.position).normalized, damage, obstacles).point);
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