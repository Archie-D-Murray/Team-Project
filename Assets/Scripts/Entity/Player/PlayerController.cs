using System;
using System.Linq;

using Attack;

using Items;

using UI;

using UnityEngine;

namespace Entity.Player {
    public class PlayerController : MonoBehaviour {
        [Serializable] private enum PlayerClass { Ranged, Melee, Mage }

        private IAttackSystem attackSystem;

        [SerializeField] private PlayerClass playerClass;

        private void Awake() {
            switch (playerClass) {
                case PlayerClass.Ranged:
                    BowData bowData = GetComponent<Inventory>().items.FirstOrDefault((Item item) => item.itemData is BowData).itemData as BowData;
                    if (!bowData) {
                        Debug.LogError("Could not find bow to initialise attackSystem!");
                        Destroy(this);
                        return;
                    }
                    attackSystem = new ProjectileSystem(GetComponent<Stats>(), transform, bowData);
                    break;
                default:
                    Debug.Log("Only implmented ranged!");
                    Destroy(this);
                    break;
            }
        }

        private void FixedUpdate() {
            attackSystem.FixedUpdate();
        }
    }
}