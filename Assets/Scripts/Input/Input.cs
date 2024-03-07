using System;

using UnityEngine;

namespace Utilities {
    [DefaultExecutionOrder(-100)]
    public class Input : Singleton<Input> {
        public PlayerControls playerControls;
        public Camera main;

        private void Start() {
            playerControls = new PlayerControls();
            playerControls.Enable();
            main = Camera.main;
        }

        public float AngleToMouse(Transform obj) {
            return Vector2.SignedAngle(
                (main.ScreenToWorldPoint(playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - obj.position).normalized,
                Vector2.up
            );
        }

        public Vector2 VectorToMouse(Transform obj) {
            return (main.ScreenToWorldPoint(playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - obj.position).normalized;
        }
    }
}