using System.Collections;

using UnityEngine;

namespace Utilities {
    public static class SpriteRendererExtensions {

        public static void FlashColour(this SpriteRenderer spriteRenderer, Color flashColour, Color originalColour, float duration, MonoBehaviour monoBehaviour) {
            monoBehaviour.StartCoroutine(FlashColour(spriteRenderer, flashColour, originalColour, duration));
        }

        public static void FlashDamage(this SpriteRenderer spriteRenderer, Material flashMaterial, Material originalMaterial, float duration, MonoBehaviour monoBehaviour) {
            monoBehaviour.StartCoroutine(FlashDamage(spriteRenderer, flashMaterial, originalMaterial, duration));
        }

        private static IEnumerator FlashDamage(SpriteRenderer spriteRenderer, Material flashMaterial, Material originalMaterial, float duration) {
            spriteRenderer.material = flashMaterial;
            yield return Yielders.WaitForSeconds(duration);
            spriteRenderer.material = originalMaterial;
        }

        private static IEnumerator FlashColour(SpriteRenderer spriteRenderer, Color flashColour, Color originalColour, float duration) {
            spriteRenderer.material.color = flashColour;
            yield return Yielders.WaitForSeconds(duration);
            spriteRenderer.material.color = originalColour;
        }
    }
}