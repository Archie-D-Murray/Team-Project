using UnityEngine;

namespace Items {
    public abstract class SpellData : ItemData {
        public float cooldown = 1f;
        public int manaCost = 10;
        public GameObject spell;
        public float magicModifier = 1f;
        public float duration;
        public float speed;
        public float castTime = 1f;

        public abstract void CastSpell(Vector3 position, Quaternion rotation, float magic);
    }
}