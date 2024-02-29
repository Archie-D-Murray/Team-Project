using UnityEngine;

using System.Collections;
using System;

namespace Entity.Player {
    [Serializable] public abstract class WeaponAnimator {
        [Header("Editor")]
        [SerializeField] protected bool allowMouseRotation = true;
        [SerializeField] protected float radius = 1f;
        [SerializeField] protected float rotationSpeed = 45f;

        [Header("Debug")]
        [SerializeField] protected Vector2 positionOffset;
        [SerializeField] protected float positionAngle;
        [SerializeField] protected WeaponController weaponController;

        public WeaponAnimator(WeaponController weaponController) {
            this.weaponController = weaponController;
        }

        public void Attack(float attackTime) {
            weaponController.StartCoroutine(WeaponAttack(attackTime));
        }

        public abstract void FixedUpdate();

        protected abstract IEnumerator WeaponAttack(float attackTime);
        
        protected void WeaponPositionRotation(float positionAngle, float weaponRotationOffset = 0f) {
            positionOffset = new Vector2(Mathf.Sin(positionAngle * Mathf.Deg2Rad), Mathf.Cos(positionAngle * Mathf.Deg2Rad));
            weaponController.transform.localPosition = positionOffset * radius;
            weaponController.transform.localRotation = Quaternion.Slerp(weaponController.transform.localRotation, Quaternion.AngleAxis(weaponRotationOffset - positionAngle, Vector3.forward), rotationSpeed * Time.fixedDeltaTime);
        }
    }
}