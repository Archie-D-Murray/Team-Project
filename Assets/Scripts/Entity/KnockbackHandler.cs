using UnityEngine;
using Entity;
public class KnockbackHandlder : MonoBehaviour {
    Health health;
    Rigidbody2D rb;

    void Start() {
    }

    void Knockback(float damage, Vector2 applicatorPosition) {
        rb.MovePosition(rb.position + (rb.position - applicatorPosition).normalized * damage);
    }
}