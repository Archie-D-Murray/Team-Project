using UnityEngine;
using Utilities;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Entity.Player {
    [Serializable] public class SwordAnimator : WeaponAnimator {
        [SerializeField] private float swingRotation = 180f;
        [SerializeField] private float swingDirection = 1f;
        [SerializeField] private float angleOffset = 90f;
        [SerializeField] private float spriteOffset = -45f;
        [SerializeField] private float charge = 0f;
        [SerializeField] private Quaternion stabRotation;

        [SerializeField] private List<Collider2D> enemies;
 
        [Serializable] public enum AttackState { NONE, NORMAL, STAB, CHARGE, SPIN }

        public AttackState attackState = AttackState.NONE;

        public Action<float> damageCallback = delegate { };
        public Action onAttackFinish;

        private float halfSwingRotation => 0.5f * swingDirection * swingRotation;

        public SwordAnimator(WeaponController weaponController, Sprite sprite, float radius) : base(weaponController) {
            spriteRenderer.sprite = sprite;
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
                    yield return Yielders.waitForEndOfFrame;
                    allowMouseRotation = false;
                    float startAngle = positionAngle;
                    float currentAngle = startAngle;
                    // Jump to halfway (may want to tune this)
                    float endAngle = positionAngle + swingDirection * swingRotation;
                    Debug.Log($"Starting timer with time: {attackTime}");
                    while (timer <= attackTime) {
                        timer += Time.fixedDeltaTime;
                        currentAngle = Mathf.Lerp(startAngle, endAngle, timer / attackTime);
                        weaponController.transform.localPosition = new Vector2(Mathf.Sin(currentAngle * Mathf.Deg2Rad), Mathf.Cos(currentAngle * Mathf.Deg2Rad));
                        weaponController.transform.localRotation = Quaternion.RotateTowards(weaponController.transform.localRotation, Quaternion.Euler(0f, 0f, -currentAngle + spriteOffset + Mathf.Lerp(0f, -angleOffset * swingDirection, timer / attackTime)), 360f * Time.fixedDeltaTime / attackTime);
                        yield return Yielders.waitForFixedUpdate;
                    }
                    damageCallback?.Invoke(0f);
                    swingDirection *= -1f;
                    allowMouseRotation = true;
                    onAttackFinish?.Invoke();
                    break;

                case AttackState.STAB:
                    Vector3 targetPos = weaponController.transform.parent.position + (Vector3) Utilities.Input.instance.VectorToMouse(weaponController.transform.parent) * 10f * radius;
                    Vector3 initialPos = weaponController.transform.parent.position;
                    Debug.Log($"Player pos: {weaponController.transform.parent.position}, target: {targetPos}");
                    stabRotation = Quaternion.AngleAxis(Utilities.Input.instance.AngleToMouse(weaponController.transform.parent) - spriteOffset, Vector3.back);
                    while (timer <= attackTime) {
                        timer += Time.fixedDeltaTime;
                        weaponController.transform.rotation = stabRotation;
                        weaponController.transform.position = Vector3.Lerp(initialPos, targetPos, timer / attackTime);
                        if (timer + Time.fixedDeltaTime >= attackTime) {
                            damageCallback?.Invoke(stabRotation.eulerAngles.z);
                        }
                        yield return Yielders.waitForFixedUpdate;
                    }
                    onAttackFinish?.Invoke();
                    break;

                case AttackState.CHARGE:
                    charge = 0f;
                    allowMouseRotation = false;
                    Collider2D collider = weaponController.transform.parent.GetComponent<Collider2D>();
                    Vector3 startPos = new Vector3(Mathf.Sin(positionAngle * Mathf.Deg2Rad), Mathf.Cos(positionAngle * Mathf.Deg2Rad), 0f) * 2f;
                    while (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.ReadValue<float>() != 0f) {
                        charge = Mathf.Min(charge + Time.fixedDeltaTime, 2f);
                        spriteRenderer.color = Color.Lerp(Color.white, Color.red, charge / 2f);
                        Quaternion spinRotation = Quaternion.AngleAxis(Vector2.SignedAngle(Vector2.up, weaponController.transform.localPosition) + spriteOffset, Vector3.forward);
                        Debug.Log($"Current rotation: {weaponController.transform.localRotation.eulerAngles}, Target rotation: {spinRotation.eulerAngles}");
                        Debug.Log($"Interpolated rotation: {Quaternion.RotateTowards(weaponController.transform.localRotation, spinRotation, 360f * Time.fixedDeltaTime).eulerAngles}");
                        weaponController.transform.localRotation = Quaternion.RotateTowards(weaponController.transform.localRotation, spinRotation, 360f * Time.fixedDeltaTime);
                        weaponController.transform.localPosition = Vector3.MoveTowards(weaponController.transform.localPosition, startPos, 10f * Time.fixedDeltaTime);
                        yield return Yielders.waitForFixedUpdate;
                    }
                    float angle = positionAngle;
                    float attackTick = 0f;
                    Debug.Log($"Starting spin with charge: {charge}");
                    while (timer <= charge) {
                        angle += Time.fixedDeltaTime * 360f * charge;
                        timer += Time.fixedDeltaTime;
                        attackTick += Time.fixedDeltaTime;
                        weaponController.transform.localPosition = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0f) * radius;
                        weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, (Vector2) weaponController.transform.localPosition.normalized) + spriteOffset), Time.fixedDeltaTime * rotationSpeed);
                        // weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.Euler(0f, 0f, angle), Time.fixedDeltaTime * rotationSpeed);
                        if (attackTick >= 0.25f) {
                            attackTick -= 0.25f;
                        }
                        if (timer >= charge * 0.5f) {
                            spriteRenderer.color = Color.Lerp(Color.red, Color.white, timer * 0.5f / attackTime); // Last half of spin, return to normal colour
                        }
                        damageCallback?.Invoke(charge);
                        yield return Yielders.waitForFixedUpdate;
                    }
                    spriteRenderer.color = Color.white;
                    onAttackFinish?.Invoke();
                    allowMouseRotation = true;
                    charge = 0f;
                    break;
            }
            attackState = AttackState.NONE;
            damageCallback = delegate { }; //Removes all listeners
        }

        public override void FixedUpdate() {
            if (!allowMouseRotation) { return; }
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent) - 90f * swingDirection;
            WeaponPositionRotation(positionAngle, angleOffset * swingDirection + spriteOffset);
        }

        protected override void WeaponPositionRotation(float positionAngle, float weaponRotationOffset = 0) {
            positionOffset = new Vector2(Mathf.Sin(positionAngle * Mathf.Deg2Rad), Mathf.Cos(positionAngle * Mathf.Deg2Rad));
            weaponController.transform.localPosition = Vector3.MoveTowards(weaponController.transform.localPosition, positionOffset, 10f * Time.fixedDeltaTime);
            weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.AngleAxis(weaponRotationOffset - positionAngle, Vector3.forward), rotationSpeed * Time.fixedDeltaTime);
        }

        public override void OnTriggerEnter2D(Collider2D enemy) {
            if (attackState == AttackState.NONE) {
                return;
            }
            if (!enemies.Contains(enemy)) {
                enemies.Add(enemy);
                if (enemy.TryGetComponent(out Health health)) {
                    switch (attackState) {
                        case AttackState.NORMAL:
                            // onDamage?.Invoke(health, 0f);
                            break;
                        case AttackState.CHARGE:
                            // onDamage?.Invoke(health, charge); //Would apply 0.25x modifier in damage callback function!
                            break;
                        case AttackState.STAB:
                            // onDamage?.Invoke(health, stabRotation.eulerAngles.z);
                            break;
                    }
                }
            }
        }
    }
}