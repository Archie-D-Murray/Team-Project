using System.Collections;

using UnityEngine;

using Utilities;

namespace Entity {
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MovementController))]
    public class PlayerKnockbackHandler : KnockbackHandler {
        [SerializeField] private MovementController movementController;

        protected override void Start() {
            base.Start();
            movementController = GetComponent<MovementController>();
        }

        public override void Knockback(float damage, KnockbackData data) {
            if (data.applyKnockback) {
                movementController.enabled = false;
                rb2D.AddForce((rb2D.position - data.pos).normalized * magnitudeModifier * damage, ForceMode2D.Impulse);
                StartCoroutine(ResetController());
            }
        }

        private IEnumerator ResetController() {
            yield return Yielders.WaitForSeconds(delay);
            rb2D.velocity = Vector2.zero;
            movementController.enabled = true;
        }
    }
}