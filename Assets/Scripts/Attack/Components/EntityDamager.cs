using Entity;

using UI;

using UnityEngine;

namespace Attack.Components {
    /// <summary>
    /// By assigning the physics layer of the GameObject to one that only collides with
    /// walls, 'enemy projectiles' and 'enemy entities' (this will change based whether 
    /// it was created by the player or an enemy) this component is attached to the damage 
    /// check can be simplified to just trying to get a health component on the entity
    /// </summary>
    public class EntityDamager : MonoBehaviour {
        private float damage;
        public void Init(float damage) {
            this.damage = damage;
        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.TryGetComponent(out Health health)) { // Hit an 'enemy entity'
                health.Damage(damage, transform.position);
            } else { // Hit an 'enemy projectile' or a wall
                Destroy(gameObject);
            }
        }
    }
}