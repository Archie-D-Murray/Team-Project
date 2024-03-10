using Entity;

using UI;

using UnityEngine;
namespace Attack.Components {
    public class FireballController : MonoBehaviour {

        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float radius;
        [SerializeField] private LayerMask enemyLayer;


        private void Start() {
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Enemy Projectiles");
        }

        public void Init(float damage, float speed, float radius) {
            this.damage = damage;
            this.speed = speed;
            this.radius = radius;
            GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        }

        private void OnTriggerEnter2D(Collider2D _) {

            foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer)) {
                if (coll.TryGetComponent(out Health health)) {
                    Debug.Log($"Fireball Damaged {health.name}");
                    health.Damage(damage, transform.position);
                    DamageNumberManager.instance.DisplayDamage($"{damage:0}", coll.ClosestPoint(transform.position));
                } else {
                    Destroy(coll.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }

}