using UnityEngine;
using Entity;
public class EnemyProjectile : MonoBehaviour {

    private Rigidbody2D rigidBodyComponent;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileDamage;
    
    public void Init(float projectileSpeed, float projectileDamage, Vector2 dir) {
        rigidBodyComponent.velocity = dir * projectileSpeed;
        this.projectileDamage = projectileDamage;
    }

    // Start is called before the first frame update
    private void Start() {
        rigidBodyComponent = GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.TryGetComponent(out Health health)) {
            health.Damage(projectileDamage);
        }
        Destroy(gameObject);
    }
}