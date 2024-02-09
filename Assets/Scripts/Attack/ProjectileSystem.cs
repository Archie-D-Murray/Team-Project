using Attack.Components;

using UI;

using UnityEngine;

using Utilities;

namespace Attack {
    public class ProjectileSystem : MonoBehaviour, IAttackSystem {
        private CountDownTimer attackTimer = new CountDownTimer(0f);
        private Stats stats;

        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float projectileSpeed = 10f;

        private void Start() {
            stats = GetComponent<Stats>();
            ResetAttackTimer();
        }

        private void FixedUpdate() {
            attackTimer.Update(Time.fixedDeltaTime);
            if (attackTimer.IsFinished && Input.instance.playerControls.Gameplay.MousePosition.enabled) {
                //Attack();
                ResetAttackTimer();
            }
        }

        public void Attack() {
            if (stats.GetStat(StatType.Damage, out float damage)) {
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(transform.up));
                projectile.AddComponent<EntityDamager>().Init(damage);
                projectile.AddComponent<LinearProjectileMover>().Init(projectileSpeed);
            } else {
                Debug.LogError("Stats does not contain a Damage entry!");
                Destroy(this);
            }
        }

        private void ResetAttackTimer() {
            if (stats.GetStat(StatType.AttackSpeed, out float attackCooldown)) {
                attackTimer.Reset(attackCooldown);
                attackTimer.Start();
            } else {
                Debug.LogError("Stats does not contain an AttackSpeed entry!");
                Destroy(this);
                return;
            }
        }
    }
}