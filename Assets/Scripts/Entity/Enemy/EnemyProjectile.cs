using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
public class EnemyProjectile : MonoBehaviour
{

    private Transform playerTransform;
    private Rigidbody2D rigidBodyComponent;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    public void SetDamage(float damage) {
        projectileDamage = damage;
    }


    // Start is called before the first frame update
    private void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().OrNull()?.transform ?? null;
        if (!playerTransform) {
            Debug.LogError("Could not find player!");
            Destroy(this);
        }
        projectileSpeed = 2.0f;
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");
        rigidBodyComponent = GetComponent<Rigidbody2D>();

        Vector3 projectileDirection = playerTransform.position - transform.position;

        rigidBodyComponent.velocity = new Vector2(projectileDirection.x, projectileDirection.y).normalized * projectileSpeed;
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (1 << collision.gameObject.layer != playerLayer.value) {
            return;
        }
        if (collision.gameObject.TryGetComponent(out Health health)) {
            health.Damage(projectileDamage);
            Destroy(this.gameObject);
        }
    }
}
