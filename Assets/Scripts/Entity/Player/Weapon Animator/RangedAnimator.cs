using System;
using System.Collections;

using UnityEngine;

using Utilities;

namespace Entity.Player {
    [Serializable] public class RangedAnimator : WeaponAnimator {

        [SerializeField] private Animator animator;

        private readonly int attack = Animator.StringToHash("Attack");

        public RangedAnimator(WeaponController weaponController) : base(weaponController) {
            animator = weaponController.GetComponent<Animator>();
        }

        public override void FixedUpdate() {
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent);
            WeaponPositionRotation(positionAngle, 0f);
        }

        protected override IEnumerator WeaponAttack(float attackTime) {
            animator.speed = 1 / attackTime;
            animator.SetTrigger(attack);
            yield return Yielders.WaitForSeconds(attackTime);
            animator.speed = 1f;
        }
    }
}