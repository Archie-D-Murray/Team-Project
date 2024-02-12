using System;

using Attack.Components;

using Entity;

using Items;

using UnityEngine;

using Utilities;

namespace Attack {
    public class ProjectileSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;
        private BowData bow;
        private GameObject projectilePrefab;

        public ProjectileSystem(Stats stats, Transform origin, BowData bow) {
            this.stats = stats;
            this.projectilePrefab = bow.projectile;
            this.origin = origin;
            this.bow = bow;
            ResetAttackTimer();
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (attackTimer.isFinished && Input.instance.playerControls.Gameplay.Attack.IsPressed()) {
                Attack();
                ResetAttackTimer();
            }
        }

        public void Attack() {
            if (stats.GetStat(StatType.Damage, out float damage)) {
                Quaternion rotation = Quaternion.AngleAxis(
                    Vector2.SignedAngle(
                        Vector2.up, 
                        (Input.instance.main.ScreenToWorldPoint(Input.instance.playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - origin.position).normalized),
                    Vector3.forward
                );
                GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, origin.position, rotation);
                projectile.GetOrAddComponent<EntityDamager>().Init(damage * bow.damageModifier);
                projectile.GetOrAddComponent<LinearProjectileMover>().Init(bow.projectileSpeed);
                projectile.GetOrAddComponent<AutoDestroy>().Init(bow.missileDuration);
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