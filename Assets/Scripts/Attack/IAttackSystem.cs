using System;

using Items;

using UnityEngine;

namespace Attack {
    public interface IAttackSystem {
        void Attack(Transform origin);
        void FixedUpdate();
        ItemData GetWeapon();
        void SetWeapon<T>(T item) where T : ItemData;
    }
}