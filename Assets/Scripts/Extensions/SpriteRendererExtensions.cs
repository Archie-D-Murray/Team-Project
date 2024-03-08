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

        public static void FadeColour(this SpriteRenderer spriteRenderer, Color colour, float duration, MonoBehaviour monoBehaviour) {
            monoBehaviour.StartCoroutine(FadeColour(spriteRenderer, colour, duration));
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

        private static IEnumerator FadeColour(SpriteRenderer spriteRenderer, Color colour, float duration) {
            float timer = 0f;
            Color originalColour = spriteRenderer.color;
            while (spriteRenderer.color != colour) {
                spriteRenderer.color = Color.Lerp(originalColour, colour, timer / duration);
                timer += Time.fixedDeltaTime;
                yield return Yielders.waitForFixedUpdate;
            }
        }
    }
}