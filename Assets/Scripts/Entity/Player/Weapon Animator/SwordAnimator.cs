using UnityEngine;
using Utilities;
using System.Collections;
using System;

namespace Entity.Player {
    [Serializable] public class SwordAnimator : WeaponAnimator {
        [SerializeField] private float swingRotation = 180f;
        [SerializeField] private float swingDirection = 1f;
        [SerializeField] private float angleOffset = 90f;

        [Serializable] public enum AttackState { NONE, NORMAL, STAB, CHARGE, SPIN }

        public AttackState attackState = AttackState.NONE;

        public Action<float> damageCallback = delegate { };

        private float halfSwingRotation => 0.5f * swingDirection * swingRotation;

        public SwordAnimator(WeaponController weaponController, Sprite sprite, float radius) : base(weaponController) {
            weaponController.GetComponent<SpriteRenderer>().sprite = sprite;
            this.radius = radius;
        }

        // protected override IEnumerator WeaponAttack(float attackTime) {
        //     // TODO: Fix this!
        //     allowMouseRotation = false;
        //     float startAngle = positionAngle;
        //     float endAngle = positionAngle - halfSwingRotation;
        //     float currentAngle = startAngle;
        //     CountDownTimer timer = new CountDownTimer(attackTime * 0.125f);
        //     for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Swing to start pos
        //         WeaponPositionRotation(currentAngle, Mathf.Lerp(Mathf.Sign(currentAngle) * angleOffset, 0f, timer.Progress()));
        //         timer.Update(Time.fixedDeltaTime);
        //         yield return Yielders.waitForFixedUpdate;
        //     }
        //     startAngle = endAngle;
        //     endAngle += swingRotation * swingDirection;
        //     timer.Restart(attackTime * 0.375f);
        //     for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Attack swing
        //         WeaponPositionRotation(currentAngle, 0f);
        //         timer.Update(Time.fixedDeltaTime);
        //         yield return Yielders.waitForFixedUpdate;
        //     }
        //     startAngle = endAngle;
        //     endAngle = positionAngle;
        //     timer.Restart(attackTime * 0.5f);
        //     for (; timer.isRunning; currentAngle = Mathf.Lerp(startAngle, endAngle, timer.Progress())) { // Swing back to start pos
        //         WeaponPositionRotation(currentAngle, Mathf.Lerp(0f, Mathf.Sign(currentAngle) * angleOffset, timer.Progress()));
        //         timer.Update(Time.fixedDeltaTime);
        //         yield return Yielders.waitForFixedUpdate;
        //     }
        //     swingDirection *= -1.0f;
        //     allowMouseRotation = true;
        // }

        protected override IEnumerator WeaponAttack(float attackTime) {
            float timer = 0f;
            if (attackState == AttackState.NONE) {
                Debug.LogError("Attack has been called without setting state, this will do nothing!");
                yield break;
            }
            switch (attackState) {
                case AttackState.NORMAL:
                    attackTime -= Time.deltaTime;
                    allowMouseRotation = false;
                    float startAngle = positionAngle + halfSwingRotation * 0.25f;
                    float currentAngle = startAngle;
                    // Jump to halfway (may want to tune this)
                    float endAngle = positionAngle + swingDirection * swingRotation;
                    Debug.Log($"Starting timer with time: {attackTime}");
                    while (timer <= attackTime) {
                        timer += Time.fixedDeltaTime;
                        currentAngle = Mathf.Lerp(startAngle, endAngle, timer / attackTime);
                        WeaponPositionRotation(currentAngle, Mathf.Lerp(0f, -angleOffset * swingDirection, timer / attackTime));
                        yield return Yielders.waitForFixedUpdate;
                    }
                    damageCallback?.Invoke(0f);
                    swingDirection *= -1f;
                    allowMouseRotation = true;
                    break;

                case AttackState.STAB:
                    Vector3 targetPos = weaponController.transform.parent.position + (Vector3) Utilities.Input.instance.VectorToMouse(weaponController.transform.parent) * 10f * radius;
                    Vector3 initialPos = weaponController.transform.parent.position;
                    Debug.Log($"Player pos: {weaponController.transform.parent.position}, target: {targetPos}");
                    Quaternion rotation = Quaternion.AngleAxis(Utilities.Input.instance.AngleToMouse(weaponController.transform.parent), Vector3.back);
                    while (timer <= attackTime) {
                        timer += Time.fixedDeltaTime;
                        weaponController.transform.rotation = rotation;
                        weaponController.transform.position = Vector3.Lerp(initialPos, targetPos, timer / attackTime);
                        if (timer + Time.fixedDeltaTime >= attackTime) {
                            damageCallback?.Invoke(rotation.eulerAngles.z);
                        }
                        yield return Yielders.waitForFixedUpdate;
                    }
                    break;

                case AttackState.CHARGE:
                    float charge = 0f;
                    while (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.ReadValue<float>() != 0f) {
                        charge = Mathf.Min(charge + Time.fixedDeltaTime, 2f);
                        yield return Yielders.waitForFixedUpdate;
                    }
                    float angle = positionAngle;
                    while (timer <= attackTime * charge) {
                        angle += Time.fixedDeltaTime * charge * 360f;
                        timer += Time.fixedDeltaTime;
                        Debug.Log($"Angle: {angle}");
                        WeaponPositionRotation(angle, 0f);
                        damageCallback?.Invoke(charge);
                        yield return Yielders.waitForFixedUpdate;
                    }
                    break;
            }
            attackState = AttackState.NONE;
            damageCallback = delegate { }; //Removes all listeners
        }

        public override void FixedUpdate() {
            if (!allowMouseRotation) { return; }
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent) - 90f * swingDirection;
            WeaponPositionRotation(positionAngle, angleOffset * swingDirection);
        }

        protected override void WeaponPositionRotation(float positionAngle, float weaponRotationOffset = 0) {
            positionOffset = new Vector2(Mathf.Sin(positionAngle * Mathf.Deg2Rad), Mathf.Cos(positionAngle * Mathf.Deg2Rad));
            weaponController.transform.localPosition = Vector3.MoveTowards(weaponController.transform.localPosition, positionOffset * radius, 10f * Time.fixedDeltaTime);
            weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.AngleAxis(weaponRotationOffset - positionAngle, Vector3.forward), rotationSpeed * Time.fixedDeltaTime);
        }
    }
}