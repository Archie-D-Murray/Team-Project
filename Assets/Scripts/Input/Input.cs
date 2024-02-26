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
                Vector2.up, 
                (main.ScreenToWorldPoint(playerControls.Gameplay.MousePosition.ReadValue<Vector2>()) - obj.position).normalized
            );
        }
    }
}