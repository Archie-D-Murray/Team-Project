using System.Linq;

using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class HealthBar : MonoBehaviour {
        [Header("Debug")]
        [SerializeField] private Image image;
        [SerializeField] private float currentHealth;

        [Header("Editor")]
        [SerializeField] private float lerpSpeed = 5f;
        [SerializeField] private Health health;

        private void Start() {
            image = GetComponentsInChildren<Image>().FirstOrDefault((Image image) => image.type == Image.Type.Filled);
            if (!image) {
                Debug.LogError("Could not find filled image in children to use as health bar!");
                Destroy(this);
                return;
            }
            if (health == null) {
                Debug.LogError("Health was not assigned in the editor!");
                Destroy(this);
                return;
            }
            currentHealth = health.percentHealth;
        }

        private void FixedUpdate() {
            currentHealth = Mathf.MoveTowards(currentHealth, health.percentHealth, lerpSpeed * Time.fixedDeltaTime);
            image.fillAmount = currentHealth;
        }

    }
        public class Health : MonoBehaviour {
            public float percentHealth => currentHealth / maxHealth;
            public float maxHealth;
            public float currentHealth;
        }
}