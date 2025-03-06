using UnityEngine;

namespace Attack.Components {
    public class LinearProjectileMover : MonoBehaviour {
        public void Init(float speed) {
            GetComponent<Rigidbody2D>().velocity = speed * transform.up;
        }
    }
}