using UnityEngine;
using Utilities;
using Data;

namespace Entity {
    [RequireComponent(typeof(Health))]
    public class DamageFlash : MonoBehaviour {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Material originalMaterial;
        [SerializeField, Range(0.125f, 5f)] private float duration = 0.25f;

        public void Start() {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            originalMaterial = spriteRenderer.material;
            GetComponent<Health>().onDamage += (float _) => Flash();
        }

        private void Flash() {
            spriteRenderer.FlashDamage(AssetServer.instance.flashMaterial, originalMaterial, duration, this);
        }
    }
}