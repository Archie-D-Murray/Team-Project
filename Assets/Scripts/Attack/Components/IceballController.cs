using Entity;

using UI;

using UnityEngine;

namespace Attack.Components {
    public class IceballController : MonoBehaviour {
        public float radius;
        public float slowAmount;
        public LayerMask enemyLayer;
        public float tickDamage;
        public float tickRate;

        private float timer = 0f;

        public void Init(float tickRate, float tickDamage, float speed, float slowAmount, float radius) {
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            this.tickRate = tickRate;
            this.tickDamage = tickDamage;
            this.slowAmount = slowAmount;
            this.radius = radius;
            GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        }

        private void FixedUpdate() {
            timer += Time.fixedDeltaTime;
            if (timer > tickRate) {
                SlowEnemies();
                timer -= tickRate;
            }
        }

        public void SlowEnemies() {
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer)) {
                if (enemy.TryGetComponent(out Health health) && enemy.TryGetComponent(out Stats stats)) {
                    health.Damage(tickDamage);
                    DamageNumberManager.instance.DisplayDamage($"{tickDamage:0}", enemy.ClosestPoint(transform.position));
                    stats.AddStatModifer(StatType.SPEED, slowAmount, tickRate);
                }
            }
        }
    }
}