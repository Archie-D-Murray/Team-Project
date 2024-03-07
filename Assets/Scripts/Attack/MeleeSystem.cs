using Items;

using UnityEngine;

using Utilities;

using Entity;
using Data;
using Entity.Player;
using System;

namespace Attack {
    public class MeleeSystem : IAttackSystem {
        private Stats stats;
        private Transform origin;
        private SwordData data;
        private WeaponController weaponController;
        private LayerMask enemyLayer;
        private bool canAttack = true;

        private SwordAnimator swordAnimator => weaponController.GetWeaponAnimator() as SwordAnimator;

        public MeleeSystem(Stats stats, Transform origin, WeaponController weaponController, SwordData data) {
            this.stats = stats;
            this.origin = origin;
            this.weaponController = weaponController;
            this.data = data;
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
            Debug.Log($"{enemyLayer.value}, {MathF.Log(enemyLayer.value, 2)}");
        }

        public void Attack(Transform origin) {
            Debug.Log("Melee Attack!");
            // GameObject.Instantiate(AssetServer.instance.slash, origin.position, origin.rotation);
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll(origin.position, data.radius, enemyLayer)) {
                if (enemy.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    health.Damage(damage * data.damageModifier, weaponController.transform.position);
                }
            }
        }

        private void StartAttack(float durationModifier = 1f) {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                weaponController.Attack(durationModifier / (attackSpeed * data.attackSpeedModifier));
            }
        }

        public void FixedUpdate() {
            if (Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.NORMAL;
                swordAnimator.damageCallback += (float _) => Attack(origin);
                swordAnimator.onAttackFinish += ResetAttackState;
                StartAttack();
                canAttack = false;
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellOne.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.STAB;
                swordAnimator.damageCallback += (float angle) => Stab(origin, angle);
                swordAnimator.onAttackFinish += ResetAttackState;
                StartAttack(1.5f);
                canAttack = false;
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.CHARGE;
                swordAnimator.onAttackFinish += ResetAttackState;
                swordAnimator.damageCallback += (float charge) => SpinAttack(charge, origin);
                StartAttack();
                canAttack = false;
            }
        }

        private void ResetAttackState() {
            canAttack = true;
        }

        private void Stab(Transform origin, float angle) {
            foreach (Collider2D enemy in Physics2D.OverlapBoxAll(weaponController.transform.position, new Vector2(data.radius * 0.5f, data.radius * 2f), weaponController.transform.eulerAngles.z, enemyLayer)) {
                if (enemy.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    health.Damage(damage * data.stabDamageModifier);
                }
            }
        }

        private void SpinAttack(float charge, Transform origin) {
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll(origin.position, data.radius, enemyLayer)) {
                if (enemy.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    health.Damage(damage * charge * data.spinDamageModifier * Time.fixedDeltaTime);
                }
            }
        }

        public ItemData GetWeapon() {
            return data;
        }

        public void SetWeapon<T>(T item) where T : ItemData {
            data = item as SwordData;
        }
    }
}