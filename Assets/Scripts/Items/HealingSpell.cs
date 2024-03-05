using Entity;

using UnityEngine;

namespace Items.Spells {
    [CreateAssetMenu(menuName = "Items/Spells/Healing Spell")]
    public class HealingSpell : SpellData {
        public LayerMask playerLayer;
        public float radius;
        public GameObject healParticles;

        public override void CastSpell(Vector3 position, Quaternion rotation, float magic) {
            foreach (Collider2D coll in Physics2D.OverlapCircleAll(position, radius, playerLayer)) {
                if (coll.gameObject.TryGetComponent(out Health health)) {
                    health.Heal(magic);
                    Instantiate(spell, position, rotation);
                }
            }
        }
    }

}