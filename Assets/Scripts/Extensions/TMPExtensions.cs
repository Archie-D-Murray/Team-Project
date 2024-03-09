using System.Collections;

using TMPro;
using UnityEngine;

namespace Utilities {
    public static class TMPExtensions {
        public static void Fade(this TMP_Text text, float duration, bool fadeToTransparent, MonoBehaviour monoBehaviour) {
            monoBehaviour.StartCoroutine(Fade(text, duration, fadeToTransparent));
        }

        private static IEnumerator Fade(TMP_Text text, float duration, bool fadeToTransparent) {
            float timer = 0f;
            while (timer <= duration) {
                timer += Time.fixedDeltaTime;
                text.alpha = fadeToTransparent ? 1f - timer / duration : timer / duration;
                yield return Yielders.waitForFixedUpdate;
            }
            text.alpha = fadeToTransparent ? 0f : 1f;
        }
    }
}