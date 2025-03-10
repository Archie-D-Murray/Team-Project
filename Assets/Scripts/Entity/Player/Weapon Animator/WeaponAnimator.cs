using UnityEngine;

using System.Collections;
using System;

namespace Entity.Player {
    [Serializable] public abstract class WeaponAnimator {
        [SerializeField] protected bool allowMouseRotation = true;
        [SerializeField] protected float radius = 1f;
        [SerializeField] protected float rotationSpeed = 45f;
        [SerializeField] protected SpriteRenderer spriteRenderer;

        protected Vector2 positionOffset;
        protected float positionAngle;
        protected WeaponController weaponController;

        public WeaponAnimator(WeaponController weaponController) {
            this.weaponController = weaponController;
            spriteRenderer = weaponController.GetComponent<SpriteRenderer>();
        }

        public void Attack(float attackTime) {
            weaponController.StartCoroutine(WeaponAttack(attackTime));
        }

        public abstract void FixedUpdate();

        protected abstract IEnumerator WeaponAttack(float attackTime);

        public virtual void OnTriggerEnter2D(Collider2D collider) { }
        
        protected virtual void WeaponPositionRotation(float positionAngle, float weaponRotationOffset = 0f) {
            positionOffset = new Vector2(Mathf.Sin(positionAngle * Mathf.Deg2Rad), Mathf.Cos(positionAngle * Mathf.Deg2Rad));
            weaponController.transform.localPosition = positionOffset;
            weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.AngleAxis(weaponRotationOffset - positionAngle, Vector3.forward), rotationSpeed * Time.fixedDeltaTime);
        }
    }
}