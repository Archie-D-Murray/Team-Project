using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Utilities;

public class Fading : MonoBehaviour {
    private Image image;
    private Color targetColour;
    private Coroutine fadeRoutine;

    public bool isFinished => fadeRoutine == null;

    void Awake() {
        image = GetComponent<Image>();
        image.color = Color.clear;
    }

    public void Fade(Color colour, float duration = 1f) {
        fadeRoutine ??= StartCoroutine(Fade(duration));
    }

    private IEnumerator Fade(float duration) {
        float timer = 0f;
        Color original = image.color;
        while (timer <= duration) {
            image.color = Color.Lerp(original, targetColour, timer / duration);
            timer += Time.fixedDeltaTime;
            yield return Yielders.waitForFixedUpdate;
        }
        fadeRoutine = null;
    }
}