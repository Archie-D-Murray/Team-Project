using Attack.Components;

using UI;

using UnityEngine;

using Utilities;

namespace Attack {
    public class ProjectileSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;

        public ProjectileSystem(Stats stats, GameObject projectilePrefab, Transform origin) {
            this.stats = stats;
            this.projectilePrefab = projectilePrefab;
            this.origin = origin;
            ResetAttackTimer();
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (attackTimer.IsFinished && Input.instance.playerControls.Gameplay.Fire.triggered) {
                Attack(origin);
                ResetAttackTimer();
            }
        }

        public void Attack(Transform origin) {
            if (stats.GetStat(StatType.Damage, out float damage)) {
                GameObject projectile = Object.Instantiate(projectilePrefab, origin.position, Quaternion.LookRotation(origin.up));
                projectile.AddComponent<EntityDamager>().Init(damage);
                projectile.AddComponent<LinearProjectileMover>().Init(projectileSpeed);
            } else {
                throw new System.Exception("Stats does not contain a Damage entry!");
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.AttackSpeed, out float attackCooldown)) {
                attackTimer.Restart(attackCooldown);
            } else {
                throw new System.Exception("Stats does not contain a Damage entry!");
            }
        }
    }
}