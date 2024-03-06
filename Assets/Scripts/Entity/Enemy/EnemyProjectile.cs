using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity.Player;
using Entity;
public class EnemyProjectile : MonoBehaviour {

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
    private void Start() {
        playerTransform = FindObjectOfType<PlayerController>().OrNull()?.transform ?? null;
        if (!playerTransform) {
            Debug.LogError("Could not find player!");
            Destroy(this);
        }
        projectileSpeed = 2.0f;
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        wallLayer = 1 << LayerMask.NameToLayer("Obstacle");
        rigidBodyComponent = GetComponent<Rigidbody2D>();

        Vector3 projectileDirection = playerTransform.position - transform.position;

        rigidBodyComponent.velocity = new Vector2(projectileDirection.x, projectileDirection.y).normalized * projectileSpeed;
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Health health)) {
            health.Damage(projectileDamage);
            Destroy(gameObject);
        } else if (1 << collision.gameObject.layer == wallLayer.value) {
            Debug.Log(collision.name);
            Destroy(gameObject);
        }
    }
}