using UnityEngine;

namespace Attack.Components {
    public class AutoDestroy : MonoBehaviour {
        private float delay;
        
        public void Init(float delay) {
            this.delay = delay;
        }

        private void Start() {
            Destroy(gameObject, delay);
        }
    }
}