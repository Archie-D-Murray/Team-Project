using Items;

using UnityEngine;

using Entity;
using Entity.Player;
using System;
using UI;

namespace Attack {
    public class MeleeSystem : IAttackSystem {
        private Stats stats;
        private Transform origin;
        private SwordData data;
        private WeaponController weaponController;
        private LayerMask enemyLayer;
        private bool canAttack = true;

        private SwordAnimator swordAnimator => weaponController.GetWeaponAnimator<SwordAnimator>();

        public MeleeSystem(Stats stats, Transform origin, WeaponController weaponController, SwordData data) {
            this.stats = stats;
            this.origin = origin;
            this.weaponController = weaponController;
            this.data = data;
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        }

        

        private void StartAttack(float durationModifier = 1f) {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                weaponController.Attack(durationModifier / (attackSpeed * data.attackSpeedModifier));
            }
        }

        public void FixedUpdate() {
            if (Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.NORMAL;
                swordAnimator.damageCallback += NormalAttack;
                swordAnimator.onAttackFinish += ResetAttackState;
                StartAttack();
                canAttack = false;
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellOne.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.STAB;
                swordAnimator.damageCallback += Stab;
                swordAnimator.onAttackFinish += ResetAttackState;
                StartAttack(1.5f);
                canAttack = false;
            } else if (Utilities.Input.instance.playerControls.Gameplay.UseSpellTwo.IsPressed() && canAttack) {
                swordAnimator.attackState = SwordAnimator.AttackState.SPIN;
                swordAnimator.onAttackFinish += ResetAttackState;
                swordAnimator.damageCallback += SpinAttack;
                StartAttack();
                canAttack = false;
            }
        }

        private void ResetAttackState() {
            canAttack = true;
        }

        public void Attack(Transform origin) { }

        private void NormalAttack(Health health, float _, Vector2 position) {
            if (stats.GetStat(StatType.DAMAGE, out float damage)) {
                health.Damage(damage * data.damageModifier, position);
                DamageNumberManager.instance.DisplayDamage($"{damage:0}", position);
            }
        }

        private void Stab(Health health, float _, Vector2 position) {
            if (stats.GetStat(StatType.DAMAGE, out float damage)) {
                health.Damage(damage * data.stabDamageModifier, position);
                DamageNumberManager.instance.DisplayDamage($"{damage:0}", position);
            }
        }

        private void SpinAttack(Health health, float charge, Vector2 position) {
            if (stats.GetStat(StatType.DAMAGE, out float damage)) {
                health.Damage(damage * charge * data.spinDamageModifier * swordAnimator.spinTickTime, position);
                DamageNumberManager.instance.DisplayDamage($"{damage:0}", position);
            }
        }

        public ItemData GetWeapon() {
            return data;
        }

        public void SetWeapon<T>(T item) where T : ItemData {
            data = item as SwordData;
            swordAnimator.SetWeapon(item as SwordData);
        }
    }
}