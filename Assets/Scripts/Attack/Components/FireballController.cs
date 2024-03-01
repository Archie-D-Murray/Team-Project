using Entity;

using UnityEngine;
namespace Attack.Components {
    public class FireballController : MonoBehaviour {

        [SerializeField] private float speed;
        [SerializeField] private float damage;
        [SerializeField] private float radius;
        [SerializeField] private Vector3 dir;
        [SerializeField] private LayerMask enemyLayer;

        [SerializeField] private Rigidbody2D rb2D;

        private void Start() {
            dir = transform.up;
            dir.z = 0f;
            rb2D = GetComponent<Rigidbody2D>();
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Enemy Projectiles");
        }

        public void Init(float damage, float speed, float radius) {
            this.damage = damage;
            this.speed = speed;
            this.radius = radius;
        }

        private void FixedUpdate() {
            rb2D.velocity = speed * Time.fixedDeltaTime * dir;
        }

        private void OnTriggerEnter2D(Collider2D _) {

            foreach (Collider2D coll in Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer)) {
                if (coll.TryGetComponent(out Health health)) {
                    Debug.Log($"Fireball Damaged {health.name}");
                    health.Damage(damage, rb2D.position);
                } else {
                    Destroy(coll.gameObject);
                }
            }
            Destroy(gameObject);
        }
    }

}