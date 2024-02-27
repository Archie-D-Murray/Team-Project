using UnityEngine;
using Attack.Components;

namespace Items.Spells {
    [CreateAssetMenu(menuName = "Items/Spells/Fireball")]
    public class FireballSpell : SpellData {

        public float radius;

        public override void CastSpell(Vector3 position, Quaternion rotation, float magic) {
            GameObject spellInstance = Instantiate(spell, position, rotation);
            spellInstance.GetOrAddComponent<FireballController>().Init(magic, speed, radius);
            spellInstance.GetOrAddComponent<AutoDestroy>().Init(duration);
        }
    }
}