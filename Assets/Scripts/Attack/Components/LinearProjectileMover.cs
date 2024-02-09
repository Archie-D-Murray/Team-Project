using UnityEngine;

namespace Attack.Components {
    public class LinearProjectileMover : MonoBehaviour {
        private float speed = 0.0f;
        private Vector2 direction;
        private Rigidbody2D rb2D;

        private void Start() {
            direction = transform.up;
            rb2D = GetComponent<Rigidbody2D>();
        }

        public void Init(float speed) {
            this.speed = speed;
        }

        private void FixedUpdate() {
            rb2D.velocity = speed * Time.fixedDeltaTime * direction;
        }
    }
}