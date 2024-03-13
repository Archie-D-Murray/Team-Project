using Entity.Player;

using UnityEngine;

using Utilities;

public class LevelDoor : MonoBehaviour {

    private void OnEnable() {
        GetComponent<SpriteRenderer>().FadeColour(Color.white, 1f, this);
        GetComponent<Collider2D>().enabled = true;
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.HasComponent<PlayerController>() && Utilities.Input.instance.playerControls.Gameplay.Interact.ReadValue<float>() != 0f) {
            GameManager.instance.LoadNextLevel();
            GetComponent<Collider2D>().enabled = false;
        }
    }
}