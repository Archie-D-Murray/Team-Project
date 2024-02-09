using System.Collections;

using UnityEngine;

using Utilities;

public static class CanvasGroupExtensions {
    public static void FadeCanvas(this CanvasGroup canvasGroup, float duration, bool fadeToTransparent, MonoBehaviour monoBehaviour) {
        monoBehaviour.StartCoroutine(CanvasFade(canvasGroup, duration, fadeToTransparent));
    }

    private static IEnumerator CanvasFade(CanvasGroup canvasGroup, float duration, bool fadeToTransparent) {
        CountDownTimer timer = new CountDownTimer(duration);
        timer.Start();
        while (timer.IsRunning) {
            canvasGroup.alpha = timer.Progress();
            timer.Update(Time.fixedDeltaTime);
            yield return Yielders.WaitForFixedUpdate;
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
}