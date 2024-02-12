using Attack.Components;

using Items;

using UI;

using UnityEngine;

using Utilities;

namespace Attack {
    public class ProjectileSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;
        private Bow bow;
        private GameObject projectilePrefab;

        public ProjectileSystem(Stats stats, GameObject projectilePrefab, Transform origin, Bow bow) {
            this.stats = stats;
            this.projectilePrefab = projectilePrefab;
            this.origin = origin;
            this.bow = bow;
            ResetAttackTimer();
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (attackTimer.isFinished && Input.instance.playerControls.Gameplay.MousePosition.enabled) {
                Attack();
                ResetAttackTimer();
            }
        }

        public void Attack(Transform origin) {
            if (stats.GetStat(StatType.Damage, out float damage)) {
                GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, origin.position, Quaternion.LookRotation(origin.up));
                projectile.AddComponent<EntityDamager>().Init(damage * bow.damageModifier);
                projectile.AddComponent<LinearProjectileMover>().Init(bow.projectileSpeed);
            } else {
                Debug.LogError("Stats does not contain a Damage entry!");
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.AttackSpeed, out float attackCooldown)) {                
                attackTimer.Restart(attackCooldown * bow.drawTimeModifier);
            } else {
                Debug.LogError("Stats does not contain an AttackSpeed entry!");
                return;
            }
        }
    }
}