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

        public SwordAnimator(WeaponController weaponController, Sprite sprite, float radius) : base(weaponController) {
            weaponController.GetComponent<SpriteRenderer>().sprite = sprite;
            this.radius = radius;
        }

        protected override IEnumerator WeaponAttack(float attackTime) {
            // TODO: Fix this!
            allowMouseRotation = false;
            float startAngle = positionAngle;
            float endAngle = positionAngle - halfSwingRotation;
            float currentAngle = startAngle;
            CountDownTimer timer = new CountDownTimer(attackTime * 0.125f);
            for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Swing to start pos
                WeaponPositionRotation(currentAngle, Mathf.Lerp(Mathf.Sign(currentAngle) * angleOffset, 0f, timer.Progress()));
                timer.Update(Time.fixedDeltaTime);
                yield return Yielders.waitForFixedUpdate;
            }
            startAngle = endAngle;
            endAngle += swingRotation * swingDirection;
            timer.Restart(attackTime * 0.375f);
            for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Attack swing
                WeaponPositionRotation(currentAngle, 0f);
                timer.Update(Time.fixedDeltaTime);
                yield return Yielders.waitForFixedUpdate;
            }
            startAngle = endAngle;
            endAngle = positionAngle;
            timer.Restart(attackTime * 0.5f);
            for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Swing back to start pos
                WeaponPositionRotation(currentAngle, Mathf.Lerp(0f, Mathf.Sign(currentAngle) * angleOffset, timer.Progress()));
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