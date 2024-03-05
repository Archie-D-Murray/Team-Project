using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fading : MonoBehaviour {
    private bool black = false;
    public bool fading { get; private set; }
    private Image image;
    private float targetAlpha;
    private float currentAlpha;

    void Awake()
    {
        image = GetComponent<Image>();
        image.color = new Color(0, 0, 0, 0);
        fading = false;
    }

    private void OnEnable() 
    {
        DoFade();
    }

    public void DoFade()
    {
        if (!fading)
        {
            StartCoroutine(Fade());
        }
    }

    private IEnumerator Fade() 
    {
        fading = true;
        if (black)
        {
            currentAlpha = 1f;
            targetAlpha = 0f;
        } else 
        {
            currentAlpha = 0f;
            targetAlpha = 1f;
        }
        while (Mathf.Abs(currentAlpha - targetAlpha) > 0f)
        {

            currentAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, 0.5f * Time.deltaTime);
            image.color = new Color(0, 0, 0, currentAlpha);
            yield return null;
        }
        black = !black;
        if (!black) 
        {
            gameObject.SetActive(false);
        }
        fading = false;
    }
}
