using Items;

using UnityEngine;

using Utilities;

using Entity;
using Data;
using Entity.Player;
using System;

namespace Attack {
    public class MeleeSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;
        private SwordData data;
        private WeaponController weaponController;
        private LayerMask enemyLayer;

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

        private void ResetAttackTimer(float cooldownModifier = 1f) {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                attackTimer.Restart(cooldownModifier / (attackSpeed * data.attackSpeedModifier));
                weaponController.Attack(cooldownModifier / (attackSpeed * data.attackSpeedModifier));
            }
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed() && attackTimer.isFinished) {
                swordAnimator.attackState = SwordAnimator.AttackState.NORMAL;
                swordAnimator.damageCallback += (float _) => Attack(origin);
                ResetAttackTimer();
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellOne.IsPressed() && attackTimer.isFinished) {
                swordAnimator.attackState = SwordAnimator.AttackState.STAB;
                swordAnimator.damageCallback += (float angle) => Stab(origin, angle);
                ResetAttackTimer(1.5f);
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.IsPressed() && attackTimer.isFinished) {
                swordAnimator.attackState = SwordAnimator.AttackState.CHARGE;
                swordAnimator.damageCallback += (float charge) => SpinAttack(charge, origin);
                ResetAttackTimer(2.0f);
            }
        }

        private void Stab(Transform origin, float angle) {
            Debug.Log($"Enemy layer: {enemyLayer.value} ({Mathf.Log(enemyLayer.value, 2)})");
            foreach (Collider2D enemy in Physics2D.OverlapBoxAll(weaponController.transform.position, new Vector2(data.radius * 0.5f, data.radius * 2f), weaponController.transform.eulerAngles.z, enemyLayer)) {
                if (enemy.transform == origin) {
                    Debug.Log("Attack has hit player?");
                    Debug.Log($"Player layer: {1 << enemy.gameObject.layer} == {enemyLayer.value}?");
                    continue;
                }
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