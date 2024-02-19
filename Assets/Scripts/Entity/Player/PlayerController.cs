using System;
using System.Linq;

using Attack;

using Items;

using UnityEngine;

namespace Entity.Player {
    public class PlayerController : MonoBehaviour {
        [Serializable] private enum PlayerClass { Ranged, Melee, Mage }

        [SerializeField] private IAttackSystem attackSystem;

        [SerializeField] private PlayerClass playerClass;

        [SerializeField] private Animator animator;

        [SerializeField] private Rigidbody2D rb2D;

        [SerializeField] private Vector2 lastDir;

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
            animator = GetComponentInChildren<Animator>();
            rb2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate() {
            attackSystem.FixedUpdate();
            animator.SetFloat("x", rb2D.velocity.normalized.x);
            animator.SetFloat("y", rb2D.velocity.normalized.y);
            animator.SetFloat("dirX", Mathf.Sign(lastDir.x));
            animator.SetFloat("dirY", Mathf.Sign(lastDir.y));
            animator.SetFloat("speed", Vector2.ClampMagnitude(rb2D.velocity, 1f).magnitude);
            if (rb2D.velocity.sqrMagnitude > 0.01f) {
                lastDir = rb2D.velocity.normalized;
            }
        }

        public void SetBow(BowData bowData) {
            attackSystem.SetWeapon(bowData);
        }
    }
}