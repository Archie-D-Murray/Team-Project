using UnityEngine;
using System;

namespace Entity {
    public class ShootingEnemy : EnemyScript {

        public GameObject projectile;

        protected override void InitEnemy() {
            type = EnemyType.SHOOTING;
            id = HashCode.Combine(type.ToString(), name);
        }
    }
}