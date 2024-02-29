using UnityEngine;
using Utilities;
using System.Collections;
using System;

namespace Entity.Player {
    [Serializable] public class SwordAnimator : WeaponAnimator {
        [Header("Editor")]
        [SerializeField] private float swingRotation = 180f;
        [SerializeField] private float swingDirection = 1f;
        [SerializeField] private float angleOffset = 90f;

        private float halfSwingRotation => 0.5f * swingDirection * swingRotation;

        public SwordAnimator(WeaponController weaponController) : base(weaponController) {}

        protected override IEnumerator WeaponAttack(float attackTime) {
            allowMouseRotation = false;
            float attackAngle = positionAngle - halfSwingRotation;
            float currentAngle = positionAngle;
            CountDownTimer timer = new CountDownTimer(attackTime * 0.125f);
            for (; currentAngle == attackAngle; currentAngle = Mathf.LerpAngle(currentAngle, attackAngle, timer.Progress())) { // Swing to start pos
                WeaponPositionRotation(currentAngle, Mathf.LerpAngle(0f, -swingDirection * angleOffset, timer.Progress()));
                timer.Update(Time.fixedDeltaTime);
                yield return Yielders.waitForFixedUpdate;
            }
            timer.Restart(attackTime * 0.375f);
            for (; attackAngle < positionAngle + 0.5f * swingRotation; attackAngle = Mathf.LerpAngle(attackAngle, positionAngle + halfSwingRotation, timer.Progress())) { // Attack swing
                WeaponPositionRotation(attackAngle, Mathf.LerpAngle(-swingDirection * angleOffset, swingRotation * angleOffset, timer.Progress()));
                timer.Update(Time.fixedDeltaTime);
                yield return Yielders.waitForFixedUpdate;
            }
            currentAngle = attackAngle;
            timer.Restart(attackTime * 0.5f);
            for (; currentAngle == positionAngle; currentAngle = Mathf.LerpAngle(currentAngle, positionAngle, timer.Progress())) { // Swing back to start pos
                WeaponPositionRotation(currentAngle, Mathf.LerpAngle(swingDirection * angleOffset, 0f, timer.Progress()));
                timer.Update(Time.fixedDeltaTime);
                yield return Yielders.waitForFixedUpdate;
            }
            swingDirection *= -1.0f;
            allowMouseRotation = true;
        }

        public override void FixedUpdate() {
            if (!allowMouseRotation) { return; }
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent);
            WeaponPositionRotation(positionAngle, angleOffset);
        }

    }
}