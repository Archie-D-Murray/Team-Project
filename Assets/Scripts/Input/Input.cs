using UnityEngine;

using Utilities;

public class Input : Singleton<Input> {
    public PlayerControls playerControls;
    public Camera main;

    private void Start() {
        playerControls = new PlayerControls();
        playerControls.Enable();
        main = Camera.main;
    }
}