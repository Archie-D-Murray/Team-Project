using UnityEngine;
using Attack.Components;

namespace Items.Spells {
    [CreateAssetMenu(menuName = "Items/Spells/Ice Ball")]
    public class IceballSpell : SpellData {

        public float radius;
        public float tickRate;
        public float slowAmount;

        public override void CastSpell(Vector3 position, Quaternion rotation, float magic) {
            GameObject spellInstance = Instantiate(spell, position, rotation);
            spellInstance.GetOrAddComponent<IceballController>().Init(tickRate, magic, speed, slowAmount, radius);
            spellInstance.GetOrAddComponent<AutoDestroy>().Init(duration);
        }
    }
}