using Utilities;
using UnityEngine;
using TMPro;

namespace UI {
    [RequireComponent(typeof(Canvas))]
    public class DamageNumberManager : Singleton<DamageNumberManager> {
        [SerializeField] private TMP_Text[] damageNumberPool = new TMP_Text[20];
        [SerializeField] private GameObject damageNumberPrefab;

        private Canvas canvas;
        private int index = 0;

        private void Start() {
            canvas = GetComponent<Canvas>();
            canvas.worldCamera = Utilities.Input.instance.main;
            for (int i = 0; i < damageNumberPool.Length; i++) {
                damageNumberPool[i] = Instantiate(damageNumberPrefab, canvas.transform).GetComponent<TMP_Text>();
                damageNumberPool[i].enabled = false;
                damageNumberPool[i].alpha = 0f;
                damageNumberPool[i].text = string.Empty;
            }
        }

        public void DisplayDamage(string text, Vector3 pos, float duration = 0.5f) {
            Init(damageNumberPool[index], pos, duration, text);
            index = ++index % damageNumberPool.Length;
        }

        private void Init(TMP_Text damageNumber, Vector3 pos, float duration, string text) {
            damageNumber.transform.position = pos;
            damageNumber.enabled = true;
            damageNumber.text = text;
            damageNumber.alpha = 1f;
            damageNumber.Fade(duration, true, this);
        }
    }
}