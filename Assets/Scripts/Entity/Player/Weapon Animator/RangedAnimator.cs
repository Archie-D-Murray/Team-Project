using System;
using System.Collections;

using UnityEngine;
using UnityEngine.U2D;

using Utilities;

namespace Entity.Player {
    [Serializable] public class RangedAnimator : WeaponAnimator {

        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Sprite[] frames;

        public RangedAnimator(WeaponController weaponController, Sprite[] frames) : base(weaponController) {
            Array.ForEach(frames, (Sprite frame) => Debug.Log($"Frame {frame.name}"));
            this.frames = frames;
            renderer = weaponController.GetComponent<SpriteRenderer>();
            renderer.sprite = frames[frames.Length - 1];
        }

        public override void FixedUpdate() {
            positionAngle = Utilities.Input.instance.AngleToMouse(weaponController.transform.parent);
            WeaponPositionRotation(positionAngle, 0f);
        }

        protected override IEnumerator WeaponAttack(float attackTime) {
            renderer.sprite = frames[0];
            CountDownTimer frameTimer = new CountDownTimer(attackTime / (frames.Length - 1));
            int frame = 0;
            while (frame != frames.Length) {
                frameTimer.Update(Time.fixedDeltaTime);
                if (frameTimer.isFinished && frame < frames.Length) {
                    frame++;
                    if (frame < frames.Length) {
                    renderer.sprite = frames[frame];
                    } else {
                        break;
                    }
                    frameTimer.Reset();
                    frameTimer.Start();
                }
                yield return Yielders.waitForFixedUpdate;
            }
            renderer.sprite = frames[frames.Length - 1];
        }
    }
}