using System;

using Attack.Components;

using Entity;
using Entity.Player;

using Items;

using UnityEngine;

using Utilities;

namespace Attack {
    [Serializable] public class RangedSystem : IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;
        private Transform origin;
        private WeaponController weaponController;
        private BowData bow;
        private GameObject projectilePrefab;

        public RangedSystem(Stats stats, Transform origin, WeaponController weaponController, BowData bow) {
            this.stats = stats;
            this.origin = origin;
            this.weaponController = weaponController;
            if (bow && bow is BowData) {
                this.projectilePrefab = bow.projectile;
                this.bow = bow;
                ResetAttackTimer();
            } else {
                Debug.LogError("Ranged System was initialised incorrectly!");
            }
        }

        public void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (attackTimer.isFinished && Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed()) {
                Attack(origin);
                ResetAttackTimer();
            }
        }

        public void Attack(Transform origin) {
            if (stats.GetStat(StatType.DAMAGE, out float damage)) {
                float angleIncrement = bow.spreadAngle / ((float) bow.projectiles - 1f);
                float spreadStart = -bow.spreadAngle * 0.5f + Utilities.Input.instance.AngleToMouse(origin);
                for (int i = 0; i < bow.projectiles; i++) {
                    Quaternion rotation = Quaternion.AngleAxis(
                        spreadStart,
                        Vector3.back
                    );
                    GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, origin.position, rotation);
                    projectile.GetOrAddComponent<EntityDamager>().Init(damage * bow.damageModifier);
                    projectile.GetOrAddComponent<LinearProjectileMover>().Init(bow.projectileSpeed);
                    projectile.GetOrAddComponent<AutoDestroy>().Init(bow.missileDuration);
                    spreadStart += angleIncrement;
                }
            } else {
                Debug.LogError("Stats does not contain a DAMAGE entry!");
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackCooldown)) {                
                attackTimer.Restart(1f / (attackCooldown * bow.drawTimeModifier));
                weaponController.Attack(1f / (attackCooldown * bow.drawTimeModifier));
            } else {
                Debug.LogError("Stats does not contain an ATTACK_SPEED entry!");
                return;
            }
        }

        public void SetWeapon<T>(T bow) where T : ItemData {
            if (bow is not BowData || !bow) {
                Debug.LogError("Tried to pass non bow to SetWeapon on ProjectileSystem!");
                return;
            }
            this.bow = bow as BowData;
            weaponController.GetWeaponAnimator<RangedAnimator>().SetWeapon(bow as BowData);
        }

        public ItemData GetWeapon() {
            return bow;
        }
    }
}