using System;
using System.Collections;

using Items;

using UnityEngine;

using Utilities;

namespace Entity.Player {

    [Serializable] public class MageStaffAnimator : WeaponAnimator {

        [SerializeField] private float angleOffset = 90f;

        public MageStaffAnimator(WeaponController weaponController, Sprite sprite) : base(weaponController) {
            weaponController.GetComponent<SpriteRenderer>().sprite = sprite;
            Debug.Log("Initialised Mage");
        }

        public override void FixedUpdate() {
            if (!allowMouseRotation) { return; }
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent);
            WeaponPositionRotation(positionAngle, angleOffset * Mathf.Sign(positionAngle));
        }

        protected override IEnumerator WeaponAttack(float attackTime) {
            allowMouseRotation = false;
            float initialAngle = weaponController.transform.localRotation.eulerAngles.z;
            float targetAngle = initialAngle - 180f;
            CountDownTimer timer = new CountDownTimer(attackTime * 0.125f);
            float angle = initialAngle;
            for (; timer.isRunning; angle = Mathf.LerpAngle(initialAngle, targetAngle, timer.Progress())) {
                timer.Update(Time.fixedDeltaTime);
                weaponController.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                yield return Yielders.waitForFixedUpdate;
            }
            float spinStartAngle = angle;
            targetAngle += 360f;
            timer.Restart(attackTime * 0.5f);
            for (; timer.isRunning; angle = Mathf.LerpAngle(spinStartAngle, targetAngle, timer.Progress())) {
                timer.Update(Time.fixedDeltaTime);
                weaponController.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                yield return Yielders.waitForFixedUpdate;
            }
            timer.Restart(attackTime * 0.25f);
            for (; timer.isRunning; angle = Mathf.LerpAngle(targetAngle, initialAngle, timer.Progress())) {
                timer.Update(Time.fixedDeltaTime);
                weaponController.transform.localRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                yield return Yielders.waitForFixedUpdate;
            }
            allowMouseRotation = true;
        }

        public void SetWeapon(MageStaffData data) {
            spriteRenderer.sprite = data.sprite;
        }
    }
}