using Items;

using UnityEngine;

using Utilities;

using Entity;

namespace Attack {
    public class MeleeSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;
        private SwordData data;
        private LayerMask enemyLayer;

        public MeleeSystem(Stats stats, Transform origin, SwordData data) {
            this.stats = stats;
            this.origin = origin;
            this.data = data;
            enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
        }

        public void Attack(Transform origin) {
            Debug.Log("Melee Attack!");
            // TODO: Play attack animation
            foreach (Collider2D enemy in Physics2D.OverlapCircleAll(origin.position, data.radius, enemyLayer)) {
                if (enemy.TryGetComponent(out Health health) && stats.GetStat(StatType.DAMAGE, out float damage)) {
                    health.Damage(damage * data.damageModifier);
                }
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackSpeed)) {
                attackTimer.Restart(1f / (attackSpeed * data.attackSpeedModifier));
            }
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed() && attackTimer.isFinished) {
                Attack(origin);
                ResetAttackTimer();
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