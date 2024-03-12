using Items;

using UnityEngine;

namespace Entity.Player {
    public class WeaponController : MonoBehaviour {
        [SerializeField, SerializeReference] private WeaponAnimator weaponAnimator;
        public T GetWeaponAnimator<T>() where T : WeaponAnimator => weaponAnimator as T;

        public void SetWeapon<T>(T weapon) where T : ItemData {
            weaponAnimator = weapon.InferItemType() switch {
                ItemType.MELEE  => new SwordAnimator(this, (weapon as SwordData).sprite, (weapon as SwordData).radius),
                ItemType.RANGED => new RangedAnimator(this, (weapon as BowData).frames),
                ItemType.MAGE => new MageStaffAnimator(this, (weapon as MageStaffData).sprite),
                _ => null
            };
        }

        private void FixedUpdate() {
            weaponAnimator?.FixedUpdate();
        }

        public void Attack(float attackTime) {
            weaponAnimator?.Attack(attackTime);
        }

        private void OnTriggerEnter2D(Collider2D collider) {
            weaponAnimator?.OnTriggerEnter2D(collider);
        }

        private bool IsSameType(ItemType type) {
            if (weaponAnimator == null) {
                return false;
            } else {
            return weaponAnimator is SwordAnimator     && type == ItemType.MELEE
                || weaponAnimator is RangedAnimator    && type == ItemType.RANGED
                || weaponAnimator is MageStaffAnimator && type == ItemType.MAGE;
            }
        }
    }
}