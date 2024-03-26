using System;
using System.Collections;

using Items;

using UnityEngine;

using Utilities;

namespace Entity.Player {
    [Serializable] public class RangedAnimator : WeaponAnimator {

        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Sprite[] frames;

        public RangedAnimator(WeaponController weaponController, Sprite[] frames) : base(weaponController) {
            Array.ForEach(frames, (Sprite frame) => Debug.Log($"Frame {frame.name}"));
            this.frames = frames;
            renderer = weaponController.GetComponent<SpriteRenderer>();
            renderer.sprite = frames[^1];
        }

        public override void FixedUpdate() {
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent);
            WeaponPositionRotation(positionAngle, 0f);
        }

        public void SetWeapon(BowData data) {
            frames = data.frames;
            spriteRenderer.sprite = data.frames[^1];
        }

        protected override IEnumerator WeaponAttack(float attackTime) {
            renderer.sprite = frames[0];
            float timer = 0f;
            while (timer < attackTime) {
                timer += Time.fixedDeltaTime;
                renderer.sprite = frames[Mathf.Clamp(Mathf.FloorToInt(3f * (timer / attackTime)), 0, frames.Length - 1)];
                yield return Yielders.waitForFixedUpdate;
            }
            renderer.sprite = frames[^1];
        }
    }
}