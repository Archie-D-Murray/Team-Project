using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagesCollectable : MonoBehaviour
{
    [SerializeField] private GameObject UIPage;
    [SerializeField] private Sprite[] images;

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.CompareTag("Player"))
        {
            UIPage.SetActive(true);
            UIPage.GetComponent<Image>().sprite = images[Random.Range(0,images.Length)];
            UIPage.GetComponent<FadeMcBob>().FadeInOut();
        }
    }
}
