using System;
using System.Linq;

using Attack;

using Items;

using UnityEngine;

using Data;

namespace Entity.Player {
    public class PlayerController : MonoBehaviour, ISerialize {
        [Serializable] private enum PlayerClass { Ranged, Melee, Mage }

        [SerializeField] private IAttackSystem attackSystem;

        [SerializeField] private PlayerClass playerClass;

        [SerializeField] private Animator animator;

        [SerializeField] private Rigidbody2D rb2D;

        [SerializeField] private Vector2 lastDir;

        [SerializeField] private Stats stats;

        private void Awake() {
            animator = GetComponentInChildren<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
            stats = GetComponent<Stats>();
            attackSystem = null;
        }

        public void DebugInitialise() {
            switch (playerClass) {
                case PlayerClass.Ranged:
                    BowData bowData = GetWeapon<BowData>();
                    if (!bowData) {
                        Debug.LogWarning("Could not find bow to initialise attackSystem, attacks will not work until this is initialised!");
                    }
                    attackSystem = new ProjectileSystem(stats, transform, bowData);
                    break;

                case PlayerClass.Melee:
                    SwordData swordData = GetWeapon<SwordData>();
                    if (!swordData) {
                        Debug.LogWarning("Could not find sword to initialise attackSystem, attacks will not work until this is initialised!");
                    }
                    attackSystem = new MeleeSystem(stats, transform, swordData);
                    break;

                default:
                    Debug.Log("Only implmented ranged!");
                    Destroy(this);
                    break;
            }
        }

        private void FixedUpdate() {
            attackSystem?.FixedUpdate();
            animator.SetFloat("x", rb2D.velocity.normalized.x);
            animator.SetFloat("y", rb2D.velocity.normalized.y);
            animator.SetFloat("dirX", Mathf.Sign(lastDir.x));
            animator.SetFloat("dirY", Mathf.Sign(lastDir.y));
            animator.SetFloat("speed", Vector2.ClampMagnitude(rb2D.velocity, 1f).magnitude);
            if (rb2D.velocity.sqrMagnitude > 0.01f) {
                lastDir = rb2D.velocity.normalized;
            }
        }

        public void OnSerialize(ref GameData data) {
            data.playerPos = rb2D.position;
            data.weaponIndex = attackSystem != null ? Array.FindIndex(GetComponent<Inventory>().items, (Item item) => item.itemData.id == attackSystem.GetWeapon().id) : -1;
        }

        public void OnDeserialize(GameData data) {
            rb2D.MovePosition(data.playerPos);
            Inventory inventory = GetComponent<Inventory>();
            if (data.weaponIndex == -1) {
                attackSystem = null;
                return;
            }
            switch (playerClass) {
                case PlayerClass.Ranged:
                    SetWeapon<BowData>(inventory.items[data.weaponIndex].itemData as BowData);
                    break;
                case PlayerClass.Melee:
                    SetWeapon<SwordData>(inventory.items[data.weaponIndex].itemData as SwordData);
                    break;
                default:
                    break;
            }
        }

        ///<summary>Gets first weapon of type T in inventory</summary>
        ///<returns>Item or null if not found</returns>
        public T GetWeapon<T>() where T : ItemData {
            Inventory inventory = GetComponent<Inventory>();
            return (inventory.items.FirstOrDefault((Item item) => item.itemData is T)?.itemData ?? null) as T;
        }

        ///<summary>Sets weapon of attack system, initialises it if it was null or attack system has changed</summary>
        public void SetWeapon<T>(T itemData) where T : ItemData {
            if (itemData) {
                if (attackSystem != null && attackSystem != null ? attackSystem.GetWeapon().GetType() == typeof(T) : false) {
                    attackSystem.SetWeapon(itemData as T);
                } else {
                    ReInitialiseAttackSystem<T>(itemData as T);
                }
            } else {
                attackSystem = null;
            }
        }

        public void ReInitialiseAttackSystem<T>(T itemData) where T : ItemData {
            switch (playerClass) {
                case PlayerClass.Ranged:
                    attackSystem = new ProjectileSystem(stats, transform, itemData as BowData);
                    break;

                case PlayerClass.Melee:
                    attackSystem = new MeleeSystem(stats, transform, itemData as SwordData);
                    break;
                default:
                    break;
            }
        }
    }
}