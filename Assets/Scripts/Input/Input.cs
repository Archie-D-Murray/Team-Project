using UnityEngine;

using Utilities;
[DefaultExecutionOrder(-100)]
public class Input : Singleton<Input> {
    public PlayerControls playerControls;
    public Camera main;

    private void Start() {
        playerControls = new PlayerControls();
        playerControls.Enable();
        main = Camera.main;
    }
}