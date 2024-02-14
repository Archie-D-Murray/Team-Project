using System.Collections;

using UnityEngine;

using Utilities;

public static class CanvasGroupExtensions {
    public static void FadeCanvas(this CanvasGroup canvasGroup, float duration, bool fadeToTransparent, MonoBehaviour monoBehaviour) {
        monoBehaviour.StartCoroutine(CanvasFade(canvasGroup, duration, fadeToTransparent));
    }

    public static void FadeAlpha(this CanvasGroup canvasGroup, float duration, bool fadeToTransparent, MonoBehaviour monoBehaviour) {
        monoBehaviour.StartCoroutine(AlphaFade(canvasGroup, duration, fadeToTransparent));
    }
    private static IEnumerator CanvasFade(CanvasGroup canvasGroup, float duration, bool fadeToTransparent) {
        CountDownTimer timer = new CountDownTimer(duration);
        timer.Start();
        while (timer.isRunning) {
            canvasGroup.alpha = fadeToTransparent ? 1f - timer.Progress() : timer.Progress();
            timer.Update(Time.fixedDeltaTime);
            yield return Yielders.waitForFixedUpdate;
        }
        if (fadeToTransparent) {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0f;
        } else {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
    }

    private static IEnumerator AlphaFade(CanvasGroup canvasGroup , float duration, bool fadeToTransparent) {
        CountDownTimer timer = new CountDownTimer(duration);
        timer.Start();
        while (timer.isRunning) {
            canvasGroup.alpha = fadeToTransparent ? 1f - timer.Progress() : timer.Progress();
            timer.Update(Time.fixedDeltaTime);
            yield return Yielders.waitForFixedUpdate;
        }
        canvasGroup.alpha = fadeToTransparent ? 0f : 1f;
    }
}