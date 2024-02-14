using System;

using Items;

using UnityEngine;

namespace Attack {
    public interface IAttackSystem {
        void Attack(Transform origin);
        void FixedUpdate();
        void SetWeapon(ItemData item);
    }
}