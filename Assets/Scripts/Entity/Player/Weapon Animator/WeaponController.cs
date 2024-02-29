using Items;

using UnityEngine;

namespace Entity.Player {
    public class WeaponController : MonoBehaviour {
        [SerializeField] private WeaponAnimator weaponAnimator;

        public void SetWeapon<T>(T weapon) where T : ItemData {
            weaponAnimator = weapon.InferItemType() switch {
                ItemType.MELEE  => new SwordAnimator(this, (weapon as SwordData).sprite),
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