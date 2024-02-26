using System;

using Attack.Components;

using Entity;

using Items;

using UnityEngine;

using Utilities;

namespace Attack {
    [Serializable] public class MageSystem : IAttackSystem {
        private CountDownTimer cooldown = new CountDownTimer(0f);
        private Stats stats;
        private Mana mana;
        private Transform origin;
        private BowData bow;
        private GameObject projectilePrefab;

        public MageSystem(Stats stats, Transform origin, BowData bow, Mana mana) {
            this.stats = stats;
            this.mana = mana;
            this.origin = origin;
            if (bow && bow is BowData) {
                this.projectilePrefab = bow.projectile;
                this.bow = bow;
                ResetAttackTimer();
            } else {
                Debug.LogError("Projectile System was initialised incorrectly!");
            }
        }

        public void FixedUpdate() {
            cooldown.Update(Time.fixedDeltaTime);
            if (cooldown.isFinished && Utilities.Input.instance.playerControls.Gameplay.Attack.IsPressed()) {
                Attack(origin);
                ResetAttackTimer();
            }
        }

        public void Attack(Transform origin) {
            if (stats.GetStat(StatType.DAMAGE, out float damage)) {
                Quaternion rotation = Quaternion.AngleAxis(
                    Vector2.SignedAngle(
                        Vector2.up, 
                        (Utilities.Input.instance.main.ScreenToWorldPoint(Utilities.Input.instance.playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - origin.position).normalized),
                    Vector3.forward
                );
                GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, origin.position, rotation);
                projectile.GetOrAddComponent<EntityDamager>().Init(damage * bow.damageModifier);
                projectile.GetOrAddComponent<LinearProjectileMover>().Init(bow.projectileSpeed);
                projectile.GetOrAddComponent<AutoDestroy>().Init(bow.missileDuration);
                mana.UseMana(10);
            } else {
                Debug.LogError("Stats does not contain a DAMAGE entry!");
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.ATTACK_SPEED, out float attackCooldown)) {                
                cooldown.Restart(1f / (attackCooldown * bow.drawTimeModifier));
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
        }

        public ItemData GetWeapon() {
            return bow;
        }
    }
}