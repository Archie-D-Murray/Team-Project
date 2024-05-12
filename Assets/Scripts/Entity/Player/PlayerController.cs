using System;
using System.Linq;

using Attack;

using Items;

using UnityEngine;

using Data;

using Utilities;

namespace Entity.Player {
    [Serializable] public enum PlayerClass { RANGED, MELEE, MAGE }
    public class PlayerController : MonoBehaviour, ISerialize {

        [SerializeField] private IAttackSystem attackSystem;
        [SerializeField] private PlayerClass playerClass;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb2D;
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private Vector2 lastDir;
        [SerializeField] private Stats stats;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Level level;

        public IAttackSystem getAttackSystem => attackSystem;
        public PlayerClass getPlayerClass => playerClass;
        public Action<PlayerClass> onClassChange;
        public Action<ItemData, ItemType> onItemEquip;

        private void Awake() {
            animator = GetComponentInChildren<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
            stats = GetComponent<Stats>();
            level = GetComponent<Level>();
            weaponController = GetComponentInChildren<WeaponController>();
            spriteRenderer = GetComponentsInChildren<SpriteRenderer>().First((SpriteRenderer spriteRenderer) => spriteRenderer.gameObject != weaponController.gameObject);
            attackSystem = null;
            GetComponent<Health>().onDeath += () => {
                GetComponentInChildren<SpriteRenderer>().FadeColour(Color.clear, 0.5f, this);
                Destroy(gameObject, 0.75f);
                GameManager.instance.PlayerDeath();
            };
        }

        private void Start() {
            if (!GameManager.instance) {
                DebugInitialise();
            }
        }

        public void DebugInitialise(PlayerClass? newClass = null) {
            if (newClass != null) {
                playerClass = newClass.GetValueOrDefault(playerClass);
            }
            switch (playerClass) {
                case PlayerClass.RANGED:
                    spriteRenderer.material = AssetServer.instance.rangedMaterial;
                    BowData bowData = GetWeapon<BowData>();
                    if (!bowData) {
                        Debug.LogWarning("Could not find bow to initialise attackSystem, attacks will not work until this is initialised!");
                    }
                    attackSystem = new RangedSystem(stats, transform, weaponController, bowData);
                    weaponController.SetWeapon(bowData);
                    level.SetConfig(AssetServer.instance.rangedConfig);
                    break;

                case PlayerClass.MELEE:
                    spriteRenderer.material = AssetServer.instance.meleeMaterial;
                    SwordData swordData = GetWeapon<SwordData>();
                    if (!swordData) {
                        Debug.LogWarning("Could not find sword to initialise attackSystem, attacks will not work until this is initialised!");
                    }
                    attackSystem = new MeleeSystem(stats, transform, weaponController, swordData);
                    weaponController.SetWeapon(swordData);
                    level.SetConfig(AssetServer.instance.meleeConfig);
                    break;

                case PlayerClass.MAGE:
                    spriteRenderer.material = AssetServer.instance.mageMaterial;
                    MageStaffData staffData = GetWeapon<MageStaffData>();
                    if (!staffData) {
                        Debug.LogWarning("Could not find mage staff to intialise attackSystem, attacks will not work intil this is initialised");
                    }
                    attackSystem = new MageSystem(stats, transform, weaponController, staffData, GetComponent<Mana>());
                    weaponController.SetWeapon(staffData);
                    level.SetConfig(AssetServer.instance.mageConfig);
                    break;

                default:
                    Debug.Log("Only implmented ranged!");
                    Destroy(this);
                    break;
            }
            onClassChange?.Invoke(playerClass);
        }

        private void FixedUpdate() {
            attackSystem?.FixedUpdate();
            animator.SetFloat("x", rb2D.velocity.normalized.x);
            animator.SetFloat("y", rb2D.velocity.normalized.y);
            animator.SetFloat("dirX", lastDir.x);
            animator.SetFloat("dirY", lastDir.y);
            animator.SetFloat("speed", Vector2.ClampMagnitude(rb2D.velocity, 1f).magnitude);
            if (rb2D.velocity.sqrMagnitude > 0.01f) {
                lastDir = rb2D.velocity.normalized;
            }
        }

        public void OnSerialize(ref GameData data) {
            data.playerData.playerClass = playerClass;
            data.playerData.playerPos = rb2D.position;
            data.playerData.weaponIndex = attackSystem != null ? Array.FindIndex(GetComponent<Inventory>().items, (Item item) => item.itemData.id == attackSystem.GetWeapon().id) : -1;
        }

        public void OnDeserialize(GameData data) {
            rb2D.MovePosition(data.playerData.playerPos);
            playerClass = data.playerData.playerClass;
            onClassChange?.Invoke(playerClass);
            Inventory inventory = GetComponent<Inventory>();
            if (data.playerData.weaponIndex == -1) {
                attackSystem = null;
                return;
            }
            switch (playerClass) {
                case PlayerClass.RANGED:
                    level.SetConfig(AssetServer.instance.rangedConfig);
                    spriteRenderer.material = AssetServer.instance.rangedMaterial;
                    SetWeapon<BowData>(inventory.items[data.playerData.weaponIndex].itemData as BowData);
                    weaponController.SetWeapon(inventory.items[data.playerData.weaponIndex].itemData as BowData);
                    break;
                case PlayerClass.MELEE:
                    spriteRenderer.material = AssetServer.instance.meleeMaterial;
                    level.SetConfig(AssetServer.instance.meleeConfig);
                    SetWeapon<SwordData>(inventory.items[data.playerData.weaponIndex].itemData as SwordData);
                    weaponController.SetWeapon(inventory.items[data.playerData.weaponIndex].itemData as SwordData);
                    break;
                case PlayerClass.MAGE:
                    spriteRenderer.material = AssetServer.instance.mageMaterial;
                    level.SetConfig(AssetServer.instance.mageConfig);
                    SetWeapon<MageStaffData>(inventory.items[data.playerData.weaponIndex].itemData as MageStaffData);
                    weaponController.SetWeapon(inventory.items[data.playerData.weaponIndex].itemData as MageStaffData);
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
                onItemEquip?.Invoke(itemData as T, itemData.InferItemType());
            } else {
                attackSystem = null;
                onItemEquip?.Invoke(null, ItemType.ITEM);
            }
        }

        public void ReInitialiseAttackSystem<T>(T itemData) where T : ItemData {
            switch (playerClass) {
                case PlayerClass.RANGED:
                    attackSystem = new RangedSystem(stats, transform, weaponController, itemData as BowData);
                    weaponController.SetWeapon(itemData as BowData);
                    break;

                case PlayerClass.MELEE:
                    attackSystem = new MeleeSystem(stats, transform, weaponController, itemData as SwordData);
                    weaponController.SetWeapon(itemData as SwordData);
                    break;
                case PlayerClass.MAGE:
                    attackSystem = new MageSystem(stats, transform, weaponController, itemData as MageStaffData, GetComponent<Mana>());
                    weaponController.SetWeapon(itemData as MageStaffData);
                    break;
                default:
                    break;
            }
        }
    }
}