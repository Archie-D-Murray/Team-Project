using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeMcBob : MonoBehaviour
{
    private float currentAlpha;
    private float targetAlpha;
    private Image image;
    private bool fading = false;

    public void FadeInOut()
    {
        if (fading) 
        {
            return;
        }
        image = GetComponent<Image>();
        currentAlpha = 0f;
        targetAlpha = 1f;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        fading = true;
        while (true)
        {
            currentAlpha = image.color.a;
            while (Mathf.Abs(currentAlpha - targetAlpha) > 0.05)
            {
                currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime);
                image.color = new Color(1, 1, 1, currentAlpha);
                yield return null;
            }
            image.color = new Color(1,1,1,targetAlpha);
            targetAlpha = 0f;
            currentAlpha = image.color.a;
            yield return null;
            if (currentAlpha == 0f)
            {
                break;
            }
        }
        gameObject.SetActive(false);
        fading = false;
    }
}
