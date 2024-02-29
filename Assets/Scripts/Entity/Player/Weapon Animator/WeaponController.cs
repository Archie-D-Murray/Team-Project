using UnityEngine;

namespace Entity.Player {
    public class WeaponController : MonoBehaviour {
        [SerializeField] private WeaponAnimator weaponAnimator;

        public void SetWeaponType(PlayerClass type) {
            if (IsSameType(type)) { return; }
            weaponAnimator = type switch {
                PlayerClass.Melee => new SwordAnimator(this),
                PlayerClass.Ranged => new RangedAnimator(this),
                PlayerClass.Mage => new MageStaffAnimator(this),
                _ => null
            };
        }

        private void FixedUpdate() {
            weaponAnimator?.FixedUpdate();
        }

        public void Attack(float attackTime) {
            weaponAnimator?.Attack(attackTime);
        }

        private bool IsSameType(PlayerClass type) {
            if (weaponAnimator == null) {
                return false;
            } else {
            return weaponAnimator is SwordAnimator     && type == PlayerClass.Melee
                || weaponAnimator is RangedAnimator    && type == PlayerClass.Ranged
                || weaponAnimator is MageStaffAnimator && type == PlayerClass.Mage;
            }
        }
    }
}